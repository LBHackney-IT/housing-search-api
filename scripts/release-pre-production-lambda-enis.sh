#!/bin/sh
# Release Lambda Hyperplane ENIs before Terraform deletes their security group.
# The AWS provider otherwise waits at least 45 minutes, and Lambda cannot delete
# the ENI after Serverless removal has deleted the function execution role.
set -eu

timeout_seconds="${ENI_RELEASE_TIMEOUT_SECONDS:-300}"
role_name="housing-search-api-lambdaExecutionRole"
role_path="/housing-search-api/pre-production/"
policy_arn="arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole"
trust_policy='{"Version":"2012-10-17","Statement":[{"Effect":"Allow","Principal":{"Service":"lambda.amazonaws.com"},"Action":"sts:AssumeRole"}]}'

export AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION:-eu-west-2}"
export AWS_PAGER=""

log() {
  printf '%s\n' "$*"
}

state=$(terraform state pull)
security_group_ids=$(printf '%s\n' "$state" | jq -r '
  .resources[]?
  | select(.mode == "managed" and .type == "aws_security_group")
  | .instances[]?.attributes.id
' | sed '/^$/d' | sort -u)

if [ -z "$security_group_ids" ]; then
  log "No security groups are in Terraform state."
  exit 0
fi

log "Checking Lambda ENIs on security groups: $(printf '%s' "$security_group_ids" | tr '\n' ' ')"

role_restored=false
restore_role() {
  if aws iam get-role --role-name "$role_name" >/dev/null 2>&1; then
    log "Lambda execution role ${role_name} is present."
  else
    log "Recreating ${role_name} so Lambda can release its network interface."
    aws iam create-role \
      --role-name "$role_name" \
      --path "$role_path" \
      --assume-role-policy-document "$trust_policy" >/dev/null
  fi
  aws iam attach-role-policy \
    --role-name "$role_name" \
    --policy-arn "$policy_arn" >/dev/null
  role_restored=true
}

delete_role() {
  if ! aws iam get-role --role-name "$role_name" >/dev/null 2>&1; then
    return 0
  fi

  policies=$(aws iam list-attached-role-policies --role-name "$role_name" --output json)
  for policy in $(printf '%s\n' "$policies" | jq -r '.AttachedPolicies[].PolicyArn'); do
    aws iam detach-role-policy --role-name "$role_name" --policy-arn "$policy"
  done

  inline_policies=$(aws iam list-role-policies --role-name "$role_name" --output json)
  inline_policies=$(printf '%s\n' "$inline_policies" | jq -r '.PolicyNames[]')
  for policy_name in $inline_policies; do
    aws iam delete-role-policy --role-name "$role_name" --policy-name "$policy_name"
  done

  aws iam delete-role --role-name "$role_name"
  log "Deleted Lambda execution role ${role_name}."
}

list_lambda_enis() {
  sg="$1"
  page=$(aws ec2 describe-network-interfaces \
    --filters "Name=group-id,Values=${sg}" "Name=description,Values=AWS Lambda VPC ENI*" \
    --output json)
  printf '%s\n' "$page" | jq -r --arg sg "$sg" '
    .NetworkInterfaces[]?
    | [$sg, .NetworkInterfaceId, .Status, (.Attachment.AttachmentId // "")]
    | @tsv
  '
}

eni_is_gone() {
  eni="$1"
  if aws ec2 describe-network-interfaces --network-interface-ids "$eni" >/dev/null 2>&1; then
    return 1
  fi
  return 0
}

release_eni() {
  eni="$1"
  attachment="$2"
  if [ -n "$attachment" ]; then
    log "Detaching ${eni}."
    aws ec2 detach-network-interface --attachment-id "$attachment" --force >/dev/null 2>&1 || true
  fi
  if aws ec2 delete-network-interface --network-interface-id "$eni" >/dev/null 2>&1; then
    log "Deleted Lambda ENI ${eni}."
    return 0
  fi
  if eni_is_gone "$eni"; then
    log "Lambda ENI ${eni} is already gone."
    return 0
  fi
  return 1
}

eni_file=$(mktemp)
trap 'rm -f "$eni_file"' EXIT
deadline=$(( $(date +%s) + timeout_seconds ))
cleanup_started=false

while true; do
  : >"$eni_file"
  for sg in $security_group_ids; do
    list_lambda_enis "$sg" >>"$eni_file"
  done

  if [ ! -s "$eni_file" ]; then
    log "No Lambda ENIs are blocking security group deletion."
    if [ "$cleanup_started" = true ]; then
      delete_role || log "Warning: could not delete ${role_name}."
    fi
    exit 0
  fi

  cleanup_started=true
  remaining=0
  while IFS=$(printf '\t') read -r sg eni status attachment; do
    [ -n "$eni" ] || continue
    if [ "$status" = "in-use" ]; then
      if [ "$role_restored" = false ]; then
        restore_role
      fi
      log "Security group ${sg} is blocked by Lambda ENI ${eni} (status: in-use)."
      remaining=$((remaining + 1))
      continue
    fi
    if ! release_eni "$eni" "$attachment"; then
      log "Security group ${sg} is blocked by Lambda ENI ${eni} (status: ${status})."
      remaining=$((remaining + 1))
    fi
  done <"$eni_file"

  if [ "$remaining" -eq 0 ]; then
    sleep 2
    continue
  fi

  now=$(date +%s)
  if [ "$now" -ge "$deadline" ]; then
    log "Lambda ENI cleanup timed out after ${timeout_seconds}s."
    log "The security group is still blocked. Terraform would otherwise wait at least 45 minutes on this ENI."
    while IFS=$(printf '\t') read -r sg eni status attachment; do
      [ -n "$eni" ] || continue
      log "Blocking ENI ${eni} on ${sg}: status ${status}."
    done <"$eni_file"
    if [ "$role_restored" = true ]; then
      log "Left ${role_name} in place so Lambda can release the ENI. Rerun the destroy job to check again."
    fi
    exit 1
  fi

  sleep 15
done
