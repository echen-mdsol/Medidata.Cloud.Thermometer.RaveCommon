using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.ExpendoState
{
    [TestClass]
    public class ExpendoStateAccessorTests
    {
        private IFixture _fixture;
        private IExpendoStateStorage _storage;
        private ExpendoStateAccessor _sut;
        private object _target;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _storage = _fixture.Create<IExpendoStateStorage>();
            _target = _fixture.Create<object>();
            var targetIdentity = RuntimeHelpers.GetHashCode(_target);
            _storage.Stub(x => x.GetStorage(targetIdentity)).Return(new Dictionary<string, object>());

            _sut = new ExpendoStateAccessor(_target, _storage);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Ctor_NullTarget_ShouldThrowException()
        {
            var sut = new ExpendoStateAccessor(null, _storage);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Ctor_NullStorage_ShouldThrowException()
        {
            var sut = new ExpendoStateAccessor(_target, null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Get_NullName_ShouldThrowException()
        {
            _sut.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Get_EmptyName_ShouldThrowException()
        {
            _sut.Get(" ");
        }

        [TestMethod]
        [ExpectedException(typeof (KeyNotFoundException))]
        public void Get_UnsetName_ShouldThrowException()
        {
            var propName = _fixture.Create<string>();

            _sut.Get(propName);
        }

        [TestMethod]
        public void Exists_UnsetName_ReturnFalse()
        {
            var propName = _fixture.Create<string>();

            var result = _sut.Exists(propName);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Exists_SetName_ReturnTrue()
        {
            var propName = _fixture.Create<string>();

            _sut.Set(propName, _fixture.Create<object>());
            var result = _sut.Exists(propName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_UnsetName_ShouldNotThrow()
        {
            var propName = _fixture.Create<string>();
            var inexistingProp = _fixture.Create<string>();
            _sut.Set(propName, _fixture.Create<object>());

            var result = _sut.Remove(inexistingProp);

            CollectionAssert.AreEquivalent(new[] {propName}, _sut.Keys.ToList());
        }

        [TestMethod]
        public void Remove_SetName_ShouldNotThrow()
        {
            var propName = _fixture.Create<string>();
            var removingPropName = _fixture.Create<string>();
            _sut.Set(propName, _fixture.Create<object>());
            _sut.Set(removingPropName, _fixture.Create<object>());
            CollectionAssert.AreEquivalent(new[] {propName, removingPropName}, _sut.Keys.ToList());

            var result = _sut.Remove(removingPropName);

            CollectionAssert.AreEquivalent(new[] {propName}, _sut.Keys.ToList());
        }

        [TestMethod]
        public void RemoveAll()
        {
            var identity = RuntimeHelpers.GetHashCode(_target);
            var propName1 = _fixture.Create<string>();
            var propValue1 = _fixture.Create<object>();
            var propName2 = _fixture.Create<string>();
            var propValue2 = _fixture.Create<object>();

            _sut.Set(propName1, propValue1)
                .Set(propName2, propValue2);

            Assert.AreEqual(2, _storage.GetStorage(identity).Count);

            _sut.RemoveAll();

            Assert.AreEqual(0, _storage.GetStorage(identity).Count);
        }

        [TestMethod]
        public void StaticClass_SetMultipleExpendoProperties()
        {
            var propName1 = _fixture.Create<string>();
            var propValue1 = _fixture.Create<object>();
            var propName2 = _fixture.Create<string>();
            var propValue2 = _fixture.Create<object>();

            _sut.Set(propName1, propValue1)
                .Set(propName2, propValue2);

            var result1 = _sut.Get(propName1);
            var result2 = _sut.Get(propName2);

            Assert.AreSame(propValue1, result1);
            Assert.AreSame(propValue2, result2);
            CollectionAssert.AreEquivalent(new[] {propName1, propName2}, _sut.Keys.ToList());
        }

        [TestMethod]
        public void StaticClass_SetSameExpendoPropertyTwice_ShouldGetLatterOne()
        {
            var propName = _fixture.Create<string>();
            var propValueOld = _fixture.Create<object>();
            var propValueNew = _fixture.Create<object>();

            _sut.Set(propName, propValueOld);
            var resultOld = _sut.Get(propName);
            Assert.AreSame(propValueOld, resultOld);

            _sut.Set(propName, propValueNew);
            var resultNew = _sut.Get(propName);

            Assert.AreSame(propValueNew, resultNew);
            CollectionAssert.AreEquivalent(new[] {propName}, _sut.Keys.ToList());
        }
    }
}