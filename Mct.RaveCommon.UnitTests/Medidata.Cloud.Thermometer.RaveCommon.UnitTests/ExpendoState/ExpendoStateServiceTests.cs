using System;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.ExpendoState
{
    [TestClass]
    public class ExpendoStateServiceTests
    {
        private IFixture _fixture;
        private ExpendoStateService _sut;
        private IExpendoStateStorage _storage;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _storage = _fixture.Create<IExpendoStateStorage>();
            _sut = new ExpendoStateService(_storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullStorage_ShouldThrowException()
        {
            var sut = new ExpendoStateService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForInstance_NullInstance_ShouldThrowException()
        {
            _sut.ForInstance(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ForInstance_InstanceOfString_ShouldThrowException()
        {
            var instance = _fixture.Create<string>();
            _sut.ForInstance(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ForInstance_InstanceOfType_ShouldThrowException()
        {
            var instance = _fixture.Create<Type>();
            _sut.ForInstance(instance);
        }

        [TestMethod]
        public void ForInstance_Return()
        {
            var instance = _fixture.Create<object>();
            var result = _sut.ForInstance(instance);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ExpendoStateAccessor));
        }

        [TestMethod]
        public void ForClassGeneric_Return()
        {
            var result = _sut.ForClass<object>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ExpendoStateAccessor));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForClass_NullType_ShouldThrowException()
        {
            _sut.ForClass(null);
        }

        [TestMethod]
        public void ForClass_Return()
        {
            var type = _fixture.Create<Type>();

            var result = _sut.ForClass(type);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ExpendoStateAccessor));
        }
    }
}
