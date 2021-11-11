Feature: OpenSearch is used to host the ElasticSearch clusters
  In order to improve security
  As engineers
  We'll use ensure our OpenSearch clusters are configured correctly

  Scenario: Ensure it is deployed in a VPC
    Given I have aws_elasticsearch_domain defined
    Then it must contain vpc_options

  Scenario: Ensure OpenSearch clusters are encrypted at rest
    Given I have aws_elasticsearch_domain defined
    Then it must contain encrypt_at_rest
    And its enabled property must be true

  Scenario: Ensure minimum instance count is 2
    Given I have aws_elasticsearch_domain defined
    Then it must contain cluster_config
    And its instance_count property must be greater and equal to 2
