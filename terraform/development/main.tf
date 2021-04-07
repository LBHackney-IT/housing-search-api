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
    bucket  = "terraform-state-housing-development"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/housing-search-api/state"
  }
}

/*    ELASTICSEARCH SETUP    */

data "aws_vpc" "development_vpc" {
  tags = {
    Name = "vpc-housing-development"
  }
}

data "aws_subnet_ids" "development" {
  vpc_id = data.aws_vpc.development_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

module "elasticsearch_db_development" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id           = data.aws_vpc.development_vpc.id
  environment_name = "development"
  port             = 443
  domain_name      = "housing-search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.development.ids)[0]]
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

 module "cors" {
  source = "squidfunk/api-gateway-enable-cors/aws"
  version = "0.3.1"

  api_id          = "y1e46yws9c"
  api_resource_id = "iw9jsltvlk"
}
