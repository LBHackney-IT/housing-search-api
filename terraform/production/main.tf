
provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
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
    bucket  = "terraform-state-housing-production"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/housing-search-api/state"
  }
}

/*    ELASTICSEARCH SETUP    */

data "aws_vpc" "production_vpc" {
  tags = {
    Name = "vpc-housing-production"
  }
}

data "aws_subnet_ids" "production" {
  vpc_id = data.aws_vpc.production_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

module "elasticsearch_db_production" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id           = data.aws_vpc.production_vpc.id
  environment_name = "production"
  port             = 443
  domain_name      = "housing-search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.production.ids)[0]]
  project_name     = "housing-search-api"
  es_version       = "7.8"
  encrypt_at_rest  = "false"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "1"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}

resource "aws_ssm_parameter" "search_elasticsearch_domain" {
  name = "/housing-search-api/production/elasticsearch-domain"
}
