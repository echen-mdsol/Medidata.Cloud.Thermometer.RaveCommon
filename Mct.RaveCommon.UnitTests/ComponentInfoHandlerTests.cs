using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using MockRepository = Rhino.Mocks.MockRepository;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class ComponentInfoHandlerTests
    {
        private IFixture _fixture;
        private ComponentInfoHandler _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _sut = MockRepository.GeneratePartialMock<ComponentInfoHandler>();
        }

        [TestMethod]
        public void HandlerShouldNotReturnNull()
        {
            // Arrange
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var question = _fixture.Create<IThermometerQuestion>();
            _sut.Stub(s => s.RaveComponentNames)
                .Return(new List<string> {assemblyName});

            // Act
            dynamic answer = _sut.Handler(question);

            // Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(assemblyName, (string) answer.component);
        }

        [TestMethod]
        public void HandlerShouldReturnUnknown()
        {
            // Arrange
            var question = _fixture.Create<IThermometerQuestion>();

            // Act
            var answer = _sut.Handler(question);

            // Assert
            Assert.AreEqual("Unknown Component", answer);
        }
    }
}