Feature: ReleaseExpendoStateWhenGCInstance
	In order to release expendo state of an instance when the instance is to be GCed
	As a developer
	I want release the instance expendo state in its Finalize() method

Background: 
	Given I have an expendo state service

Scenario: GC should release instance's expendo state if implemented Finalize()
	And I new an instance of the class that calls release expendo state in Finalize()
	When I set any expendo state for the instance
	Then the expendo state service should contain the instance's expendo state
	When I execute .NET garbage collection
	Then the expendo state service should not contain the instance's expendo state

Scenario: GC shouldn't release instance's expendo state if not implemented Finalize()
	And I new an instance of the class that doens't implement Finalize()
	When I set any expendo state for the instance
	Then the expendo state service should contain the instance's expendo state
	When I execute .NET garbage collection
	Then the expendo state service should contain the instance's expendo state

