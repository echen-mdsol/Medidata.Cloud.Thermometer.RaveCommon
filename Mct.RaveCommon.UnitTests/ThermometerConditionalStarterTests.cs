using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class ThermometerConditionalStarterTests
    {
        private IThermometerStarter _concreteStarter;
        private IDisposable _disposible;
        private Task<IDisposable> _disposibleTask;
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _concreteStarter = _fixture.Create<IThermometerStarter>();
            _disposible = _fixture.Create<IDisposable>();

            var tcs = new TaskCompletionSource<IDisposable>();
            tcs.SetResult(_disposible);
            _disposibleTask = tcs.Task;

            _concreteStarter.Stub(x => x.Start()).Return(_disposible);
            _concreteStarter.Stub(x => x.StartAsync()).Return(_disposibleTask);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Ctor_NullStarter()
        {
            var conditional = _fixture.Create<bool>();
            var sut = new ThermometerConditionalStarter(conditional, null);
        }

        [TestMethod]
        public void Start_ConditionalIsTrue_ShouldCallStarterStart()
        {
            // Act
            var sut = new ThermometerConditionalStarter(true, _concreteStarter);
            var result = sut.Start();

            // Assert
            _concreteStarter.AssertWasCalled(x => x.Start());
            Assert.AreEqual(_disposible, result);
        }

        [TestMethod]
        public void Start_ConditionalIsFalse_ShouldNotCallStarterStart()
        {
            // Act
            var sut = new ThermometerConditionalStarter(false, _concreteStarter);
            var result = sut.Start();

            // Assert
            _concreteStarter.AssertWasNotCalled(x => x.Start());
            Assert.AreEqual(sut, result);
        }

        [TestMethod]
        public void StartAsync_ConditionalIsTrue_ShouldCallStarterStart()
        {
            // Act
            var sut = new ThermometerConditionalStarter(true, _concreteStarter);
            var task = sut.StartAsync();
            task.Wait();

            // Assert
            _concreteStarter.AssertWasCalled(x => x.StartAsync());
            Assert.AreEqual(_disposible, task.Result);
        }

        [TestMethod]
        public void StartAsync_ConditionalIsFalse_ShouldNotCallStarterStart()
        {
            // Act
            var sut = new ThermometerConditionalStarter(false, _concreteStarter);
            var task = sut.StartAsync();
            task.Wait();

            // Assert
            _concreteStarter.AssertWasNotCalled(x => x.StartAsync());
            Assert.AreEqual(sut, task.Result);
        }
    }
}