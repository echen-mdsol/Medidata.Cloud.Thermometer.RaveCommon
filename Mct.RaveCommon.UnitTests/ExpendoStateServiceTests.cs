using System;
using System.IO;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class ExpendoStateServiceTests
    {
        private IFixture _fixture;
        private IExpendoStateStorage _storage;
        private ExpendoStateService _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _storage = _fixture.Create<IExpendoStateStorage>();
            _sut = new ExpendoStateService(_storage);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Ctor_NullAccessorFactory_ShouldThrowException()
        {
            // Arrange
            IExpendoStateAccessorFactory accessorFactory = null;

            // Act
            _sut = new ExpendoStateService(_storage, accessorFactory);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ForInstance_NullInstance_ShouldThrowException()
        {
            _sut.ForInstance(null);
        }

        [TestMethod]
        [ExpectedException(typeof (NotSupportedException))]
        public void ForInstance_InstanceOfString_ShouldThrowException()
        {
            var instance = _fixture.Create<string>();
            _sut.ForInstance(instance);
        }

        [TestMethod]
        [ExpectedException(typeof (NotSupportedException))]
        public void ForInstance_InstanceOfType_ShouldThrowException()
        {
            var instance = _fixture.Create<Type>();
            _sut.ForInstance(instance);
        }

        [TestMethod]
        public void ForInstance_Return()
        {
            // Arrage
            var instance = _fixture.Create<object>();

            // Act
            var result = _sut.ForInstance(instance);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ExpendoStateAccessor));
        }

        [TestMethod]
        public void ForClassGeneric_Return()
        {
            // Act
            var result = _sut.ForClass<object>();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ExpendoStateAccessor));
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ForClass_NullType_ShouldThrowException()
        {
            _sut.ForClass(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void ForClass_NonClassType_ShouldThrowException()
        {
            // Arrange
            var type = FileAccess.ReadWrite.GetType();

            // Act
            _sut.ForClass(type);
        }

        [TestMethod]
        public void ForClass_Return()
        {
            // Arrage
            var type = _fixture.Create<Type>();

            // Act
            var result = _sut.ForClass(type);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ExpendoStateAccessor));
        }
    }
}