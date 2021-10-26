
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
  source                 = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id                 = data.aws_vpc.production_vpc.id
  environment_name       = "production"
  port                   = 443
  domain_name            = "housing-search-api-es"
  subnet_ids             = data.aws_subnet_ids.production.ids
  project_name           = "housing-search-api"
  es_version             = "7.8"
  encrypt_at_rest        = "true"
  instance_type          = "t3.medium.elasticsearch"
  instance_count         = "2"
  ebs_enabled            = "true"
  ebs_volume_size        = "30"
  region                 = data.aws_region.current.name
  account_id             = data.aws_caller_identity.current.account_id
  zone_awareness_enabled = true
}

resource "aws_ssm_parameter" "search_elasticsearch_domain" {
  name  = "/housing-search-api/production/elasticsearch-domain"
  type  = "String"
  value = "https://vpc-housing-search-api-es-cggwz5gia7iqw6kxw64ytgrmr4.eu-west-2.es.amazonaws.com"
}

module "housing_search_api_cloudwatch_dashboard" {
  source                  = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/dashboards/api-dashboard"
  environment_name        = var.environment_name
  api_name                = "housing-search-api"
  include_sns_widget      = false
  include_dynamodb_widget = false
  no_sns_widget_dashboard = false
}
