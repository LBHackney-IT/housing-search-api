terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.0"
    }
  }
}

provider "aws" {
  region = "eu-west-2"

  default_tags {
    tags = {
      Name              = "housing-search-api-${var.environment_name}"
      Environment       = var.environment_name
      terraform-managed = true
      project_name      = var.project_name
      Application       = "MTFH Housing Pre-Production"
      TeamEmail         = "developementteam@hackney.gov.uk"
      BackupPolicy      = "Dev"
      Confidentiality   = "Internal"
    }
  }
}

data "aws_caller_identity" "current" {}

data "aws_region" "current" {}

terraform {
  backend "s3" {
    bucket         = "housing-pre-production-terraform-state"
    encrypt        = true
    region         = "eu-west-2"
    key            = "services/housing-search-api/state"
    dynamodb_table = "housing-pre-production-terraform-state-lock"
  }
}

data "aws_vpc" "pre_production_vpc" {
  tags = {
    Name = "housing-pre-prod-pre-prod"
  }
}

data "aws_subnet_ids" "pre_production" {
  vpc_id = data.aws_vpc.pre_production_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

module "elasticsearch_db_pre_production" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id           = data.aws_vpc.pre_production_vpc.id
  environment_name = "pre-production"
  port             = 443
  domain_name      = "housing-search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.pre_production.ids)[0]]
  project_name     = "housing-search-api"
  es_version       = "7.8"
  encrypt_at_rest  = "true"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "2"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}
