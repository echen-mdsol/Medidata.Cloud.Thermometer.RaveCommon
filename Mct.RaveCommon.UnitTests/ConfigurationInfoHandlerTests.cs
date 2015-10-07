using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
    [TestClass]
    public class ConfigurationInfoHandlerTests
    {
        private IFixture _fixture;
        private ConfigurationInfoHandler _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _sut = MockRepository.GeneratePartialMock<ConfigurationInfoHandler>();
        }

        [TestMethod]
        public void Handler_ReturnsDataTable()
        {
            // Arrange
            var question = _fixture.Create<IThermometerQuestion>();
            var connectionString = _fixture.Create<string>();
            _sut.Stub(x => x.GetConnectionString()).Return(connectionString);
            var expectedDataTable = new DataTable();
            var adapter = _fixture.Create<IDbDataAdapter>();
            adapter.Stub(x => x.Fill(Arg<DataSet>.Is.Anything))
                .Return(1)
                .WhenCalled(call =>
                {
                    var dataSet = (DataSet) call.Arguments.First();
                    dataSet.Tables.Add(expectedDataTable);
                    call.ReturnValue = 1;
                });
            _sut.Stub(x => x.CreateDataAdapter(
                Arg<string>.Is.Anything,
                Arg<string>.Is.Same(connectionString)))
                .Return(adapter);

            // Act
            var result = _sut.Handler(question) as DataTable;

            // Assert
            Assert.AreSame(expectedDataTable, result);
        }

        [TestMethod]
        public void CreateDataAdapter_ShouldReturnSqlDataAdapter()
        {
            // Arrange
            var sql = _fixture.Create<string>();
            var connectionString = "Server=WIN81;Database=RaveDev;uid=RaveDev;pwd=password*8";

            // Act
            var result = _sut.CreateDataAdapter(sql, connectionString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(SqlDataAdapter));
        }

        [TestMethod]
        public void GetConnectionString_ShouldReturnTheFirstConnectionString()
        {
            // Arrange
            var connectionSettings = _fixture.CreateMany<ExpandoObject>().OfType<dynamic>().ToList();
            connectionSettings.ForEach(x => x.ConnectionString = _fixture.Create<string>());
            var dataSettings = new
            {
                ConnectionSettings = connectionSettings
            };
            _sut.Stub(x => x.GetRaveDataSettingsSectionObject()).Return(dataSettings);

            // Act
            var result = _sut.GetConnectionString();

            // Assert
            var firstConnectionString = connectionSettings.Select(x => x.ConnectionString).First();
            Assert.AreEqual(firstConnectionString, result);
        }
    }
}