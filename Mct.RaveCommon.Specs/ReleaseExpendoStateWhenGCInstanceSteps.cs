using System;
using System.Runtime.CompilerServices;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Medidata.Cloud.Thermometer.RaveCommon.Specs.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Medidata.Cloud.Thermometer.RaveCommon.Specs
{
    [Binding]
    public class ReleaseExpendoStateWhenGcInstanceSteps
    {
        private ExpendoStateService _expendoStateService;
        private int _identity;
        private ExpendoStateConcurrentStorage _storage;
        private WeakReference _weakRefOfInstance;

        [Given(@"I have an expendo state service")]
        public void GivenIHaveAExpendoStateService()
        {
            _storage = new ExpendoStateConcurrentStorage();
            _expendoStateService = new ExpendoStateService(_storage);
        }

        [Given(@"I new an instance of the class that calls release expendo state in Finalize\(\)")]
        public void GivenINewAnInstanceOfTheClassThatCallsReleaseExpendoStateInFinalize()
        {
            var instance = new ReleasableClass(_expendoStateService);
            _identity = RuntimeHelpers.GetHashCode(instance);
            _weakRefOfInstance = new WeakReference(instance);
        }

        [Given(@"I new an instance of the class that doens't implement Finalize\(\)")]
        public void GivenINewAnInstanceOfTheClassThatDoensTImplementFinalize()
        {
            var instance = new object();
            _identity = RuntimeHelpers.GetHashCode(instance);
            _weakRefOfInstance = new WeakReference(instance);
        }


        [When(@"I set any expendo state for the instance")]
        public void WhenISetAnyExpendoStateForTheInstance()
        {
            var instance = _weakRefOfInstance.Target;
            _expendoStateService.ForInstance(instance).Set("State", "SomeValue");
        }

        [When(@"I execute \.NET garbage collection")]
        public void WhenIExecute_NETGarbageCollection()
        {
            GC.Collect();
            Assert.IsNull(_weakRefOfInstance.Target);
        }

        [Then(@"the expendo state service should contain the instance's expendo state")]
        public void ThenTheExpendoStateServiceShouldContainTheInstanceSExpendoState()
        {
            Assert.IsTrue(_storage.AllStorages.ContainsKey(_identity));
        }

        [Then(@"the expendo state service should not contain the instance's expendo state")]
        public void ThenTheExpendoStateServiceShouldNotContainTheInstanceSExpendoState()
        {
            Assert.IsFalse(_storage.AllStorages.ContainsKey(_identity));
        }
    }
}