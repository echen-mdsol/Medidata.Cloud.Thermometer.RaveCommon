using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

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

            var expectedResult = _fixture.Create<object>();
            _sut.Stub(x => x.FlattenToObject(expectedDataTable)).Return(expectedResult);

            // Act
            var result = _sut.Handler(question);

            // Assert
            Assert.AreSame(expectedResult, result);
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
            Assert.IsInstanceOfType(result, typeof (SqlDataAdapter));
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

        [TestMethod]
        public void FlattenToObject_ShouldReturnObject()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Tag", typeof (string));
            table.Columns.Add("ConfigValue", typeof (string));

            var count = new Random().Next(2, 5);
            for (var i = 0; i < count; i++)
            {
                var tag = _fixture.Create<string>();
                var configValue = _fixture.Create<string>();
                table.Rows.Add(tag, configValue);
            }

            // Act
            var result = _sut.FlattenToObject(table);

            // Assert
            var dic = result as IDictionary<string, object>;
            Assert.IsNotNull(dic);
            Assert.AreEqual(count, dic.Count);
            foreach (var row in table.Rows.OfType<DataRow>())
            {
                var key = row["Tag"] as string;
                var value = row["ConfigValue"];
                Assert.IsTrue(dic.ContainsKey(key));
                Assert.AreEqual(value, dic[key]);
            }
        }
    }
}