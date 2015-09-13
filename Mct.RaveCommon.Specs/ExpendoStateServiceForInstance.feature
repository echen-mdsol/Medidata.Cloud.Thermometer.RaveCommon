Feature: ExpendoStateServiceForInstance
	In order to extend state for static classes
	As a developer
	I want use ExpendoStateService on static classes

Background:
	Given I have an Object instance "cat"
	And I have an Object instance "dog"
	And I have an ExpendoStateService

Scenario: Set and get state for different instances should operate on different states.
	Given I set state "State" as "Running" for instance "cat"
	And I set state "State" as "Stopping" for instance "dog"
	When I get state "State" for instance "cat"
	Then the result should be "Running"
	When I get state "State" for instance "dog"
	Then the result should be "Stopping"

Scenario: Set and get static state for different instances should operate on the same state. 
	Given I set static state "StaticProp" as "Running" for instance "cat"
	When I get static state "StaticProp" for instance "cat"
	Then the result should be "Running"
	Given I set static state "StaticProp" as "Stopping" for instance "dog"
	When I get static state "StaticProp" for instance "dog"
	Then the result should be "Stopping"
	When I get static state "StaticProp" for instance "cat"
	Then the result should be "Stopping"