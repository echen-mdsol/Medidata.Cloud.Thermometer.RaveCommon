using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class RouteInfoHandlerTests
    {
        private IFixture _fixture;
        private IThermometerQuestion _question;
        private RouteInfoHandler _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _question = _fixture.Create<IThermometerQuestion>();
            _sut = new RouteInfoHandler();
        }

        [TestMethod]
        public void HandleQuestion_ReturnsRouteUrls()
        {
            // Arrange
            var routes = _fixture.CreateMany<Route>().ToList();
            foreach (var route in routes)
            {
                route.Url = _fixture.Create<string>();
                RouteTable.Routes.Add(route);
            }

            // Act
            var result = _sut.Handler(_question) as IEnumerable<object>;

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEquivalent(routes.Select(x => x.Url).ToArray(), result.ToArray());
        }
    }
}