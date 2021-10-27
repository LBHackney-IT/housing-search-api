
provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
  parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-housing-staging"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/housing-search-api/state"
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
  domain_name      = "housing-search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.staging.ids)[0]]
  project_name     = "housing-search-api"
  es_version       = "7.8"
  encrypt_at_rest  = "true"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "1"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}

resource "aws_ssm_parameter" "search_elasticsearch_domain" {
  name  = "/housing-search-api/staging/elasticsearch-domain"
  type  = "String"
  value = module.elasticsearch_db_staging.es_endpoint_url
}

module "housing_search_api_cloudwatch_dashboard" {
  source                  = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/dashboards/api-dashboard"
  environment_name        = var.environment_name
  api_name                = "housing-search-api"
  include_sns_widget      = false
  include_dynamodb_widget = false
  no_sns_widget_dashboard = false
}
