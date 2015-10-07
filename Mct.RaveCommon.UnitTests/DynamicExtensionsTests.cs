using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class DynamicExtensionsTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
        }

        [TestMethod]
        public void ToDynamic_ReturnsSelfIfAlreadyIsExpendo()
        {
            var expected = _fixture.Create<ExpandoObject>();

            var result = expected.ToDynamic();

            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void ToDynamic_ConvertsObjectToExpendo()
        {
            var value = _fixture.Create<string>();
            var expected = new { x = value };

            var result = expected.ToDynamic();

            Assert.AreEqual(expected.x, result.x);
        }

        [TestMethod]
        public void ToDynamic_ConvertsNull()
        {
            var result = ((object) null).ToDynamic();

            Assert.IsNull(result);
        }
    }
}