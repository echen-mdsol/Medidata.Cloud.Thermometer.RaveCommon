﻿using System;
using System.Collections.Generic;
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
    public class DbInfoHandlerTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
        }

        [TestMethod]
        public void CreateConnection_ReturnsSqlConnection()
        {
            var connectionString = "Server=WIN81;Database=RaveDev;uid=RaveDev;pwd=password*8";
            var sut = new DbInfoHandler();

            var result = sut.CreateConnection(connectionString);

            Assert.IsInstanceOfType(result, typeof (SqlConnection));
        }

        [TestMethod]
        public void GetConnectionState_ReturnsConnectionState()
        {
            var connectionString = _fixture.Create<string>();
            var state = _fixture.Create<ConnectionState>();
            var connection = _fixture.Create<IDbConnection>();
            connection.Stub(x => x.State).Return(state);

            var sut = MockRepository.GeneratePartialMock<DbInfoHandler>();
            sut.Stub(x => x.CreateConnection(connectionString)).Return(connection);

            var result = sut.GetConnectionState(connectionString);

            Assert.AreEqual(state, result);
        }

        [TestMethod]
        public void GetConnectionState_ReturnsException()
        {
            var connectionString = _fixture.Create<string>();
            var exception = _fixture.Create<Exception>();
            var connection = _fixture.Create<IDbConnection>();
            connection.Stub(x => x.Open()).Throw(exception);

            var sut = MockRepository.GeneratePartialMock<DbInfoHandler>();
            sut.Stub(x => x.CreateConnection(connectionString)).Return(connection);

            var result = sut.GetConnectionState(connectionString);

            Assert.AreEqual(exception, result);
        }

        [TestMethod]
        public void ConvertConnectionStringToObject_HidesUserIDAndPassword()
        {
            var serverName = _fixture.Create<string>();
            var dbname = _fixture.Create<string>();
            var userid = _fixture.Create<string>();
            var password = _fixture.Create<string>();
            var connectionString = string.Format("Server={0};Database={1};uid={2};pwd={3}",
                serverName,
                dbname,
                userid,
                password);
            var sut = new DbInfoHandler();

            dynamic result = sut.ConvertConnectionStringToObject(connectionString);
            IDictionary<string, object> dic = result;

            Assert.AreEqual(serverName, dic["Data Source"]);
            Assert.AreEqual(dbname, dic["Initial Catalog"]);
            Assert.IsFalse(dic.ContainsKey("User ID"));
            Assert.IsFalse(dic.ContainsKey("Password"));
        }

        [TestMethod]
        public void HandleQuestion_DataSettingsHasNoConnectionSettings()
        {
            var question = _fixture.Create<IThermometerQuestion>();
            var sut = MockRepository.GeneratePartialMock<DbInfoHandler>();

            var connectionSetting1 = new { ConnectionString = "Server=WIN81;Database=RaveDev;uid=RaveDev;pwd=password*8;" };
            var connectionSetting2 = new { ConnectionString = "Server=WIN81;Database=RaveDev;uid=RaveDev;pwd=password*8;" };

            var raveDataSettingsObject = new
            {
                ConnectionSettings = new[] {connectionSetting1, connectionSetting2}
            };

            sut.Stub(x => x.GetRaveDataSettingsSectionObject()).Return(raveDataSettingsObject);

            var connectionState = _fixture.Create<ConnectionState>();
            sut.Stub(x => x.GetConnectionState(null)).IgnoreArguments().Return(connectionState);

            var connectionStringObject = _fixture.Create<ExpandoObject>();
            sut.Stub(x => x.ConvertConnectionStringToObject(null)).IgnoreArguments().Return(connectionStringObject);

            dynamic result = sut.Handler(question);
            var resultSettings = ((IEnumerable<dynamic>)result.ConnectionSettings).ToList();

            Assert.AreEqual(raveDataSettingsObject.ConnectionSettings.Length, resultSettings.Count);

            var first = resultSettings[0];
            var second = resultSettings[1];

            Assert.AreEqual(connectionState, first.ConnectionState);
            Assert.AreEqual(connectionState, second.ConnectionState);

            Assert.AreEqual(connectionStringObject, first.ConnectionString);
            Assert.AreEqual(connectionStringObject, second.ConnectionString);

        }
    }
}