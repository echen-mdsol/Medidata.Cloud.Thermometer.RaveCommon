using System.Collections.Generic;
using Medidata.Cloud.Thermometer.RaveCommon.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.Handlers
{
    [TestClass]
    public class AssemblyInfoHandlerTests
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
            //Arrange
            var question = _fixture.Create<IThermometerQuestion>();

            //Act
            var sut = new AssemblyInfoHandler();
            var answer = sut.Handler(question);

            //Assert
            Assert.IsNotNull(answer);
            Assert.IsInstanceOfType(answer, typeof(IDictionary<string, string>));
        }
    }
}
