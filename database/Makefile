.ONESHELL:

# Requires AWS CLI Profile matching housing-${ENVIRONMENT} to be set up
# Requires AWS Session Manager Plugin to be installed:
# 	https://docs.aws.amazon.com/systems-manager/latest/userguide/session-manager-working-with-install-plugin.html
# On Windows you will need to run these commands using Git Bash, NOT Powershell / CMD

# -- Configuration --
# Local port to connect to on your machine
LOCAL_PORT = 9200
# For Parameter store URL Paths
ENVIRONMENT = development
# Set to AWSCLI Profile name
PROFILE = housing-${ENVIRONMENT}

# -- Parameter Store paths --
INSTANCE_ID_PATH := " /platform-apis-jump-box-instance-name"
ES_DOMAIN_PATH := " /housing-search-api/${ENVIRONMENT}/elasticsearch-domain"

# -- Parameters --
INSTANCE_ID := $(shell aws ssm get-parameter --name ${INSTANCE_ID_PATH} --region "eu-west-2" --profile ${PROFILE} --query Parameter.Value --output text)
ES_DOMAIN := $(shell aws ssm get-parameter --name ${ES_DOMAIN_PATH} --region "eu-west-2" --profile ${PROFILE} --query Parameter.Value --output text)
ES_DOMAIN := $(shell echo ${ES_DOMAIN} | sed 's/https:\/\///g')
ES_PORT := 443

DATABASE_PARAMS := '{"host":["${ES_DOMAIN}"], "portNumber":["${ES_PORT}"], "localPortNumber":["${LOCAL_PORT}"]}'

# -- Commands --

# Use this command to login to the AWS SSO service
sso_login:
	if (aws sts get-caller-identity --profile ${PROFILE})
	then
		echo "Session still valid"
	else
		echo "Session expired, logging in"
		aws sso login --profile ${PROFILE}
	fi

port_forwarding_to_housing_search:
	echo "Connecting to ${ES_DOMAIN_PATH} on local port ${LOCAL_PORT}\n\n==="
	aws ssm start-session \
		--target ${INSTANCE_ID} \
		--region=eu-west-2  \
		--profile=${PROFILE} \
		--document AWS-StartPortForwardingSessionToRemoteHost \
		--parameters ${DATABASE_PARAMS}

reset:
	fuser -n tcp -k ${LOCAL_PORT} && echo "Port ${LOCAL_PORT} cleared" || echo "Port ${LOCAL_PORT} not in use"
