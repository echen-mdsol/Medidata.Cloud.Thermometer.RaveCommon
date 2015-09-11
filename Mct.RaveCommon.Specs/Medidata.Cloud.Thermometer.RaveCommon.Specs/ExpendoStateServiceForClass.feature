Feature: ExpendoStateServiceForClass
	In order to extend state for static classes
	As a developer
	I want use ExpendoStateService on static classes

Background:
	Given I have a static class
	And I have an ExpendoStateService

Scenario: Set state for static class
	Given I set state "State" as "Running" for the class
	When I get state "State" for the class
	Then the result should be "Running"
