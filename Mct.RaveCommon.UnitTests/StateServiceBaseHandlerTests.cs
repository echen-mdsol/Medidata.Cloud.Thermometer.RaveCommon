using System;
using Medidata.Cloud.StateBroker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class StateServiceBaseHandlerTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture()
                .Customize(new AutoRhinoMockCustomization())
                .Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Ctor_NullStateService_ShouldThrowException()
        {
            // Arrange
            Exception expectedException = null;
            _fixture.Inject((IStateService) null);

            try
            {
                // Act
                _fixture.Create<StateServiceBaseHandler>();
            }
            catch (Exception e)
            {
                expectedException = e.InnerException;
            }

            // Assert
            Assert.IsNotNull(expectedException);
            Assert.IsInstanceOfType(expectedException, typeof (ArgumentNullException));
        }

        [TestMethod]
        public void HandleQuestion_ShouldUseTheInjectedStateService()
        {
            // Arrange
            var question = _fixture.Create<IThermometerQuestion>();
            var stateService = _fixture.Create<IStateService>();


            var sut = new Mock<StateServiceBaseHandler>(stateService) {CallBase = true};
            sut.Protected().Setup("HandleQuestion", question, stateService);

            var func = sut.Object.Handler;
            var result = func(question);

            sut.Protected().Verify("HandleQuestion", Times.Once(), question, stateService);
        }
    }
}