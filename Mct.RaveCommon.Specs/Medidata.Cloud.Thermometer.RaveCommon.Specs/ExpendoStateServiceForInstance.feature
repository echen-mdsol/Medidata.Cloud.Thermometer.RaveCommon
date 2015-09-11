Feature: ExpendoStateServiceForInstance
	In order to extend state for static classes
	As a developer
	I want use ExpendoStateService on static classes

Background:
	Given I have an Object instance "a"
	And I have an Object instance "b"
	And I have an ExpendoStateService

Scenario: Set state for different instances
	Given I set state "State" as "Running" for instance "a"
	And I set state "State" as "Stopping" for instance "b"
	When I get state "State" for instance "a"
	Then the result should be "Running"
	When I get state "State" for instance "b"
	Then the result should be "Stopping"

