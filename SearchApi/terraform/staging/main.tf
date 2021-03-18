# INSTRUCTIONS: 
# 1) ENSURE YOU POPULATE THE LOCALS 
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES 
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL - FOR APIS, THE BUCKETS ALREADY EXIST
# 5) THE VALUES OF THE COMMON COMPONENTS THAT YOU WILL NEED ARE PROVIDED IN THE COMMENTS
# 6) IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
# 7) ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY

provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
  application_name = your application name # The name to use for your application
   parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}


data "aws_iam_role" "ec2_container_service_role" {
  name = "ecsServiceRole"
}

data "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"
}

terraform {
  backend "s3" {
    bucket  = "services/search-api/state"
    encrypt = true
    region  = "eu-west-2"
    key     = services/YOUR API NAME/state #e.g. "services/transactions-api/state"
  }
}

/*    ELASTICSEARCH SETUP    */

data "aws_vpc" "staging_vpc" {
  tags = {
    Name = "vpc-housing-staging"
  }
}

data "aws_subnet_ids" "staging" {
  vpc_id = data.aws_vpc.staging_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

module "elasticsearch_db_staging" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id           = data.aws_vpc.staging_vpc.id
  environment_name = "staging"
  port             = 443
  domain_name      = "search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.staging.ids)[0]]
  project_name     = "search-api"
  es_version       = "7.8"
  encrypt_at_rest  = "false"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "1"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}

data "aws_ssm_parameter" "search_elasticsearch_domain" {
  name = "/search-api/staging/elasticsearch-domain"
}

module "staging" {
  # Delete as appropriate:
  source                      = "github.com/LBHackney-IT/aws-hackney-components-per-service-terraform.git//modules/environment/backend/fargate"
  # source = "github.com/LBHackney-IT/aws-hackney-components-per-service-terraform.git//modules/environment/backend/ec2"
  cluster_name                = "staging-apis"
  ecr_name                    = ecr repository name # Replace with your repository name - pattern: "hackney/YOUR APP NAME"
  environment_name            = "staging"
  application_name            = local.application_name 
  security_group_name         = back end security group name # Replace with your security group name, WITHOUT SPECIFYING environment. Usually the SG has the name of your API
  vpc_name                    = "vpc-staging-apis"
  host_port                   = port # Replace with the port to use for your api / app
  port                        = port # Replace with the port to use for your api / app
  desired_number_of_ec2_nodes = number of nodes # Variable will only be used if EC2 is required. Do not remove it. 
  lb_prefix                   = "nlb-staging-apis"
  ecs_execution_role          = data.aws_iam_role.ecs_task_execution_role.arn
  lb_iam_role_arn             = data.aws_iam_role.ec2_container_service_role.arn
  task_definition_environment_variables = {
    ASPNETCORE_ENVIRONMENT = "staging"
  }
  task_definition_environment_variable_count = number # This number needs to reflect the number of environment variables provided
  cost_code = your project's cost code
  task_definition_secrets      = {}
  task_definition_secret_count = number # This number needs to reflect the number of environment variables provided
}
