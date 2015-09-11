Feature: ExpendoStateServiceForInstance
	In order to extend state for static classes
	As a developer
	I want use ExpendoStateService on static classes

Background:
	Given I have an Object instance "cat"
	And I have an Object instance "dog"
	And I have an ExpendoStateService

Scenario: Set and get state for different instances
	Given I set state "State" as "Running" for instance "cat"
	And I set state "State" as "Stopping" for instance "dog"
	When I get state "State" for instance "cat"
	Then the result should be "Running"
	When I get state "State" for instance "dog"
	Then the result should be "Stopping"

