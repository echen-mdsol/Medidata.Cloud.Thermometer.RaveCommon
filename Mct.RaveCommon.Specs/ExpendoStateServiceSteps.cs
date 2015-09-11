using System;
using System.Collections.Generic;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Medidata.Cloud.Thermometer.RaveCommon.Specs.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Medidata.Cloud.Thermometer.RaveCommon.Specs
{
    [Binding]
    public class ExpendoStateServiceSteps
    {
        private ExpendoStateService _expendoStateService;
        private readonly Dictionary<string, object> _instanceDic = new Dictionary<string, object>();
        private object _result;
        private IExpendoStateAccessor _stateAccessor;
        private Type _staticClassType;

        [Given(@"I have a static class")]
        public void GivenIHaveAStaticClass()
        {
            _staticClassType = typeof (TheStaticClass);
        }

        [Given(@"I have an ExpendoStateService")]
        public void GivenIHaveAnExpendoStateService()
        {
            _expendoStateService = new ExpendoStateService();
        }

        [Given(@"I set state ""(.*)"" as ""(.*)"" for the class")]
        public void GivenISetStateAsForTheClass(string name, string value)
        {
            _expendoStateService.ForClass(_staticClassType).Set(name, value);
        }

        [Given(@"I have an Object instance ""(.*)""")]
        public void GivenIHaveAnObjectInstance(string instanceName)
        {
            _instanceDic.Add(instanceName, new object());
        }

        [Given(@"I set state ""(.*)"" as ""(.*)"" for instance ""(.*)""")]
        public void GivenISetStateAsForInstance(string name, string value, string instanceName)
        {
            var instance = _instanceDic[instanceName];
            _expendoStateService.ForInstance(instance).Set(name, value);
        }

        [When(@"I get state ""(.*)"" for instance ""(.*)""")]
        public void WhenIGetStateForInstance(string name, string instanceName)
        {
            var instance = _instanceDic[instanceName];
            _result = _expendoStateService.ForInstance(instance).Get(name);
        }

        [When(@"I get state ""(.*)"" for the class")]
        public void WhenIGetStateForTheClass(string name)
        {
            _result = _expendoStateService.ForClass(_staticClassType).Get(name);
        }

        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string value)
        {
            Assert.AreEqual(value, _result);
        }
    }
}