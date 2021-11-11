#Feature: SSM Parameter store provides a secure way to store config variables for our applications
#  In order to improve security
#  As engineers
#  We'll use AWS SSM Parameter store to store our secrets
#
#
#  Scenario: Ensure all SSM Parameters are using the SecureString type
#    Given I have aws_ssm_parameter defined
#    Then its type must be SecureString
