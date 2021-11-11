Feature: RDS is used to host our databases
  In order to improve security
  As engineers
  We'll use ensure our RDS databases are configured correctly

  Scenario: Ensure RDS instance is not publicly accessible
    Given I have aws_db_instance defined
    Then its publicly_accessible property must be false

#Feature: OpenSearch is used to host the ElasticSearch clusters
#  In order to improve security
#  As engineers
#  We'll use ensure our OpenSearch clusters are configured correctly
#
#  Scenario: Ensure OpenSearch clusters are encrypted at rest
#    Given I have aws_elasticsearch_domain defined
#    Then it must contain encrypt_at_rest
#    And it must contain true

#  Scenario: Ensure it is in a VPC
#    Given I have aws_elasticsearch_domain defined
#    Then it must contain vpc_options
#
#  Scenario: Ensure minimum instance count is 2
#    Given I have aws_elasticsearch_domain defined
#    Then it must contain cluster_config
#    And it must contain instance_count
#    And its value must be greater and equal than 2
#
#  Scenario: Ensure instance type is small or medium
#    Given I have aws_elasticsearch_domain defined
#    Then it must contain cluster_config
#    And it must contain instance_type
#    And its value must match the "^(t3\.small\.elasticsearch|t3\.medium\.elasticsearch)" regex
