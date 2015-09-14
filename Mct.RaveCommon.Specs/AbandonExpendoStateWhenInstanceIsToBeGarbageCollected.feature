Feature: AbandonExpendoStateWhenInstanceIsToBeGarbageCollected
	In order to abandon expendo state of an instance when the instance is to be GCed
	As a developer
	I want abandon the instance expendo state in its destructor

Background: 
	Given I have an expendo state service

Scenario: Expendo state should be abandoned if the instance implemented destructor
	And I new an instance of the class that implemented destructor
	When I set any expendo state for the instance
	Then the expendo state service should contain the instance's expendo state
	When I execute .NET garbage collection
	Then the instance should be garbage collected
	And the expendo state service should not contain the instance's expendo state

Scenario: Expendo state shouldn't be abandoned if the instance didn't implemente destructor
	And I new an instance of the class that didn't implement destructor
	When I set any expendo state for the instance
	Then the expendo state service should contain the instance's expendo state
	When I execute .NET garbage collection
	Then the instance should be garbage collected
	And the expendo state service should contain the instance's expendo state

