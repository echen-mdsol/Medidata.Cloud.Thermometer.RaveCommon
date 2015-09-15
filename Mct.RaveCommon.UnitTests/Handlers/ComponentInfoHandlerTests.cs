using System;
using Medidata.Cloud.Thermometer.RaveCommon.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.Handlers
{
    [TestClass]
    public class ComponentInfoHandlerTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
        }

        [TestMethod]
        public void HandlerShouldNotReturnNull()
        {
            var question = _fixture.Create<IThermometerQuestion>();

            var sut = new ComponentInfoHandler();
            var answer = sut.Handler(question);

            Assert.IsNotNull(answer);
        }
    }
}
