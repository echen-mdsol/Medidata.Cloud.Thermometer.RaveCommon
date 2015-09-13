using System.Linq;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.ExpendoState
{
    [TestClass]
    public class ExpendoStateConcurrentStorageTests
    {
        private IFixture _fixture;
        private ExpendoStateConcurrentStorage _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _sut = new ExpendoStateConcurrentStorage();
        }

        [TestMethod]
        public void GetStorage_ReturnNewIfNotExisting()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act
            _sut.GetStorage(identity);

            // Assert
            Assert.AreEqual(1, _sut.AllStorages.Count);
            Assert.AreEqual(identity, _sut.AllStorages.First().Key);
        }

        [TestMethod]
        public void GetStorage_ReturnIfExisting()
        {
            // Arrange
            var identity = _fixture.Create<int>();
            _sut.GetStorage(identity);
            var expectedStorage = _sut.AllStorages.First().Value;

            // Act
            var result = _sut.GetStorage(identity);

            // Assert
            Assert.AreEqual(1, _sut.AllStorages.Count);
            Assert.AreEqual(identity, _sut.AllStorages.First().Key);
            Assert.AreSame(expectedStorage, result);
        }

        [TestMethod]
        public void ReleaseStorage_ExistingTarget()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act and Assert
            _sut.GetStorage(identity);
            Assert.AreEqual(1, _sut.AllStorages.Count);

            _sut.ReleaseStorage(identity);
            Assert.AreEqual(0, _sut.AllStorages.Count);
        }

        [TestMethod]
        public void ReleaseStorage_NonExistingTarget()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act and Assert
            _sut.GetStorage(identity);
            Assert.AreEqual(1, _sut.AllStorages.Count);

            var deletingIdentity = _fixture.Create<int>();
            _sut.ReleaseStorage(deletingIdentity);
            Assert.AreEqual(1, _sut.AllStorages.Count);
        }
    }
}