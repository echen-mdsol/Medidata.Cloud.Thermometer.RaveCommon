using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Reflection;
using Medidata.Cloud.Thermometer.RaveCommon.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.Handlers
{
    [TestClass]
    public class ComponentInfoHandlerTests : ComponentInfoHandler
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
            // Arrange
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var question = _fixture.Create<IThermometerQuestion>();
            var sut = MockRepository.GeneratePartialMock<ComponentInfoHandlerTests>();
            sut.Stub(s => s.RaveComponentNames)
                .Return(new List<string> { assemblyName });

            // Act
            dynamic answer = sut.Handler(question);

            // Assert
            Assert.IsNotNull((object)answer);
            Assert.AreEqual((string)answer.component, assemblyName);
        }

        [TestMethod]
        public void HandlerShouldReturnUnknown()
        {
            // Arrange
            var question = _fixture.Create<IThermometerQuestion>();
            var sut = MockRepository.GeneratePartialMock<ComponentInfoHandlerTests>();

            // Act
            var answer = sut.Handler(question);

            // Assert
            Assert.AreEqual(answer, "Unknown Component");
        }
    }
}
