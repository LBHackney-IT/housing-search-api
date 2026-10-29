#!/bin/sh
# Release Lambda Hyperplane ENIs before Terraform deletes their security group.
# Lambda keeps these ENIs in use until it deletes them, and it can do that only
# after a function using the same security group is deleted while its execution
# role still exists. Recreating the role alone does not restart that cleanup.
set -eu

timeout_seconds="${ENI_RELEASE_TIMEOUT_SECONDS:-1200}"
role_name="housing-search-api-lambdaExecutionRole"
role_path="/housing-search-api/pre-production/"
policy_arn="arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole"
trust_policy='{"Version":"2012-10-17","Statement":[{"Effect":"Allow","Principal":{"Service":"lambda.amazonaws.com"},"Action":"sts:AssumeRole"}]}'
cleanup_function="housing-search-api-eni-cleanup"
cleanup_function_created=false
rearmed=false
zip_path=""

export AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION:-eu-west-2}"
export AWS_PAGER=""

log() {
  printf '%s\n' "$*"
}

remove_cleanup_function() {
  if [ "$cleanup_function_created" != true ]; then
    return 0
  fi
  aws lambda delete-function --function-name "$cleanup_function" >/dev/null 2>&1 || true
  cleanup_function_created=false
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
    log "Recreating ${role_name} so Lambda can release its network interfaces."
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
    | [
        $sg,
        .NetworkInterfaceId,
        .Status,
        (.Attachment.AttachmentId // ""),
        (.SubnetId // ""),
        (.Description // "")
      ]
    | @tsv
  '
}

list_functions_using_sg() {
  sg="$1"
  marker=""
  while true; do
    if [ -n "$marker" ]; then
      page=$(aws lambda list-functions --marker "$marker" --output json)
    else
      page=$(aws lambda list-functions --output json)
    fi
    printf '%s\n' "$page" | jq -r --arg sg "$sg" --arg cleanup "$cleanup_function" '
      .Functions[]?
      | select(.FunctionName != $cleanup)
      | select(any(.VpcConfig.SecurityGroupIds[]?; . == $sg))
      | .FunctionName
    '
    marker=$(printf '%s\n' "$page" | jq -r '.NextMarker // empty')
    [ -n "$marker" ] || break
  done
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

rearm_lambda_cleanup() {
  restore_role
  role_json=$(aws iam get-role --role-name "$role_name" --output json)
  role_arn=$(printf '%s\n' "$role_json" | jq -r '.Role.Arn')
  subnets=$(cut -f5 "$eni_file" | sed '/^$/d' | sort -u | tr '\n' ',' | sed 's/,$//')
  groups=$(cut -f1 "$eni_file" | sed '/^$/d' | sort -u | tr '\n' ',' | sed 's/,$//')

  if [ -z "$subnets" ] || [ -z "$groups" ]; then
    log "Could not determine the subnets or security groups for the blocking ENIs."
    exit 1
  fi

  log "Creating temporary Lambda ${cleanup_function} on subnets ${subnets} so Lambda releases these ENIs."
  aws lambda delete-function --function-name "$cleanup_function" >/dev/null 2>&1 || true

  zip_path=$(mktemp)
  python3 - "$zip_path" << 'PY'
import sys, zipfile
with zipfile.ZipFile(sys.argv[1], "w") as archive:
    archive.writestr("index.py", "def handler(event, context):\n    return 'ok'\n")
PY

  attempts=0
  while [ "$attempts" -lt 12 ]; do
    if aws lambda create-function \
      --function-name "$cleanup_function" \
      --runtime python3.12 \
      --role "$role_arn" \
      --handler index.handler \
      --zip-file "fileb://${zip_path}" \
      --timeout 3 \
      --memory-size 128 \
      --vpc-config "SubnetIds=${subnets},SecurityGroupIds=${groups}" \
      >/dev/null; then
      cleanup_function_created=true
      break
    fi
    attempts=$((attempts + 1))
    if [ "$attempts" -eq 12 ]; then
      log "Could not create temporary Lambda ${cleanup_function}."
      exit 1
    fi
    sleep 5
  done

  state_deadline=$(( $(date +%s) + 180 ))
  while true; do
    config=$(aws lambda get-function-configuration --function-name "$cleanup_function" --output json)
    function_state=$(printf '%s\n' "$config" | jq -r '.State')
    state_reason=$(printf '%s\n' "$config" | jq -r '.StateReason // empty')
    log "Temporary Lambda ${cleanup_function} is ${function_state}."
    if [ "$function_state" = "Active" ]; then
      break
    fi
    if [ "$function_state" = "Failed" ]; then
      log "Temporary Lambda failed: ${state_reason}"
      exit 1
    fi
    if [ "$(date +%s)" -ge "$state_deadline" ]; then
      log "Temporary Lambda did not become active."
      exit 1
    fi
    sleep 5
  done

  aws lambda delete-function --function-name "$cleanup_function"
  cleanup_function_created=false
  log "Deleted temporary Lambda ${cleanup_function}. Waiting up to ${timeout_seconds}s for the ENIs to be released."
}

eni_file=$(mktemp)
trap 'rm -f "$eni_file"; if [ -n "$zip_path" ]; then rm -f "$zip_path"; fi; remove_cleanup_function' EXIT
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
  if [ "$rearmed" = false ]; then
    other_functions=""
    for sg in $security_group_ids; do
      other_functions="${other_functions}$(list_functions_using_sg "$sg")
"
    done
    other_functions=$(printf '%s\n' "$other_functions" | sed '/^$/d' | sort -u)
    if [ -n "$other_functions" ]; then
      log "These Lambda functions still use the security group, so its ENIs remain in use:"
      printf '%s\n' "$other_functions"
      exit 1
    fi
    rearm_lambda_cleanup
    rearmed=true
    deadline=$(( $(date +%s) + timeout_seconds ))
    continue
  fi

  remaining=0
  while IFS=$(printf '\t') read -r sg eni status attachment subnet description; do
    [ -n "$eni" ] || continue
    if [ "$status" = "in-use" ]; then
      log "Security group ${sg} is blocked by Lambda ENI ${eni} on ${subnet} (status: in-use)."
      remaining=$((remaining + 1))
      continue
    fi
    if ! release_eni "$eni" "$attachment"; then
      log "Security group ${sg} is blocked by Lambda ENI ${eni} on ${subnet} (status: ${status})."
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
    log "Lambda did not release these ENIs after the temporary function was deleted."
    while IFS=$(printf '\t') read -r sg eni status attachment subnet description; do
      [ -n "$eni" ] || continue
      log "Blocking ENI ${eni} on ${sg} subnet ${subnet}: status ${status} (${description})."
    done <"$eni_file"
    if [ "$role_restored" = true ]; then
      log "Left ${role_name} in place so Lambda can finish releasing the ENIs."
    fi
    exit 1
  fi

  sleep 15
done
