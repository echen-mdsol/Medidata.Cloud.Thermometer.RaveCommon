using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.ExpendoState
{
    [TestClass]
    public class ExpendoStateInstanceAccessorTests
    {
        private IExpendoStateAccessorFactory _accessorFactory;
        private IFixture _fixture;
        private IExpendoStateStorage _storage;
        private ExpendoStateInstanceAccessor _sut;
        private object _target;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _storage = _fixture.Create<IExpendoStateStorage>();
            _target = _fixture.Create<object>();
            var targetIdentity = RuntimeHelpers.GetHashCode(_target);
            _storage.Stub(x => x.GetStorage(targetIdentity)).Return(new Dictionary<string, object>());

            _accessorFactory = _fixture.Create<IExpendoStateAccessorFactory>();

            _sut = new ExpendoStateInstanceAccessor(_target, _storage, _accessorFactory);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Ctor_NullAccessorFactory_ShouldThrowException()
        {
            // Arrange
            _accessorFactory = null;

            // Act
            _sut = new ExpendoStateInstanceAccessor(_target, _storage, _accessorFactory);
        }

        [TestMethod]
        public void StaticProperty()
        {
            // Act
            var result = _sut.Static;

            // Assert
            _accessorFactory.AssertWasCalled(x => x.CreateAccessor(_target.GetType(), _storage));
        }

        [TestMethod]
        public void Abandon_ShouldAbandonTheStorage()
        {
            // Arrange
            var targetIdentity = RuntimeHelpers.GetHashCode(_target);

            // Act
            _sut.Abandon();

            // Assert
            _storage.AssertWasCalled(x => x.AbandonStorage(targetIdentity));
        }
    }
}