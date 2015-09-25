using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using MockRepository = Rhino.Mocks.MockRepository;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
	[TestClass]
	public class ConfigurationInfoHandlerTests
	{
		private IFixture _fixture;

		[TestInitialize]
		public void Init()
		{
			_fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
		}

		[TestMethod]
		public void ConvertToExpendoObject_ReturnsSelfIfAlreadyIsExpendo()
		{
			var expected = _fixture.Create<ExpandoObject>();
			var sut = new ConfigurationInfoHandler();

			var result = sut.ConvertToExpendoObject(expected);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ConvertToExpendoObject_ConvertsObjectToExpendo()
		{
			var value = _fixture.Create<string>();
			var expected = new { x = value , list = new List<int>(){0,1,2,3,4,5,6,7}};
			var sut = new ConfigurationInfoHandler();

			dynamic result = sut.ConvertToExpendoObject(expected);

			Assert.AreEqual(expected.x, result.x);
			Assert.AreEqual(expected.list.Count, result.list.Count);
			Assert.AreEqual(expected.list[2], result.list[2]);
		}

		[TestMethod]
		public void CreateConnection_ReturnsSqlConnection()
		{
			var connectionString = "Server=WIN81;Database=RaveDev;uid=RaveDev;pwd=password*8";
			var sut = new ConfigurationInfoHandler();

			var result = sut.CreateConnection(connectionString);

			Assert.IsInstanceOfType(result, typeof(SqlConnection));
		}


		[TestMethod]
		[ExpectedException(typeof(Exception),"Bad DataReader.Read()")]
		public void GetConfigurationInfoFromDb_FailedReader()
		{
			//Arrange
			IDataReader dr = MockRepository.GenerateStub<IDataReader>();
			dr.Stub(x => x.Read()).Throw(new Exception("Bad DataReader.Read()"));

			//Act
			var sut = new ConfigurationInfoHandler();
			sut.GetConfigurationInfoFromDb(dr);

			//Assert
			
		}

		[TestMethod]
		public void GetConfigurationInfoFromDb_NoRecords()
		{
			//Arrange
			IDataReader dr = MockRepository.GenerateStub<IDataReader>();
			dr.Stub(x => x.Read()).Return(false);

			//Act
			var sut = new ConfigurationInfoHandler();
			List<dynamic> result = sut.GetConfigurationInfoFromDb(dr);
			
			//Assert
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void GetConfigurationInfoFromDb_ReturnSomeRecords()
		{
			//Arrange
			IDataReader dr = MockRepository.GenerateStub<IDataReader>();
			dr.Stub(x => x.Read()).Repeat.Twice().Return(true);
			dr.Stub(x => x.Read()).Return(false);
			dr.Stub(x => x["ID"]).Repeat.Once().Return(1);
			dr.Stub(x => x["Tag"]).Repeat.Once().Return("Tag1");
			dr.Stub(x => x["ConfigValue"]).Repeat.Once().Return("Value1");
			dr.Stub(x => x["ID"]).Repeat.Once().Return(2);
			dr.Stub(x => x["Tag"]).Repeat.Once().Return("Tag2");
			dr.Stub(x => x["ConfigValue"]).Repeat.Once().Return("Value2");


			//Act
			var sut = new ConfigurationInfoHandler();
			List<dynamic> result = sut.GetConfigurationInfoFromDb(dr);

			//Assert
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(1, result[0].ID);
			Assert.AreEqual("Tag1", result[0].Tag);
			Assert.AreEqual("Value1", result[0].ConfigValue);
			Assert.AreEqual(2, result[1].ID);
			Assert.AreEqual("Tag2", result[1].Tag);
			Assert.AreEqual("Value2", result[1].ConfigValue);
		}


		[TestMethod]
		public void HandleQuestion_ReturnListOfResult()
		{
			//Arrange
			var question = _fixture.Create<IThermometerQuestion>();
			var sut = MockRepository.GeneratePartialMock<ConfigurationInfoHandler>();

			var raveDataSettingsObject = _fixture.Create<object>();
			sut.Stub(x => x.GetRaveDataSettingsSectionObject()).Return(raveDataSettingsObject);

			dynamic connectionSetting1 = new ExpandoObject();
			connectionSetting1.ConnectionString = _fixture.Create<string>();
			connectionSetting1.DataSourceHint = "RaveDB";
			dynamic connectionSetting2 = new ExpandoObject();
			connectionSetting2.ConnectionString = _fixture.Create<string>();
			connectionSetting2.DataSourceHint = "RaveDB_Reporting";

			
			dynamic expendo = _fixture.Create<ExpandoObject>();
			expendo.ConnectionSettings = new List<object> { connectionSetting1, connectionSetting2 };
			
			dynamic defaultHint = _fixture.Create<ExpandoObject>();
			defaultHint.Value = "RaveDB";
			expendo.DefaultHint = defaultHint;
			sut.Stub(x => x.ConvertToExpendoObject(raveDataSettingsObject)).Return(expendo);

			var conn = _fixture.Create<IDbConnection>();
			sut.Stub(x => x.CreateConnection(null)).IgnoreArguments().Return(conn);

			var reader = _fixture.Create<IDataReader>();
			sut.Stub(x => x.GetDataReaderByConnection(null)).IgnoreArguments().Return(reader);

			List<object> list = new List<object>{new {ID = 0, Tag = "Tag0", Value = "Value0"}};
			sut.Stub(x => x.GetConfigurationInfoFromDb(reader)).IgnoreArguments().Return(list);

			//Act
			dynamic result = sut.Handler(question);

			//Assert
			Assert.AreSame(result, list);
		}


		[TestMethod]
		public void HandleQuestion_ThrowsExceptionWhenGettingConfigSection()
		{
			//Arrange
			var question = _fixture.Create<IThermometerQuestion>();
			var sut = MockRepository.GeneratePartialMock<ConfigurationInfoHandler>();
			sut.Stub(x => x.GetRaveDataSettingsSectionObject()).Throw(new Exception("Error when reading DataSettings section"));

			//Act
			dynamic result = sut.Handler(question);
			
			//Assert
			Assert.IsInstanceOfType(result, typeof(Exception));
			Assert.AreEqual("Error when reading DataSettings section", result.Message);
		}

		[TestMethod]
		public void HandleQuestion_NoRecord()
		{
			//Arrange
			var question = _fixture.Create<IThermometerQuestion>();
			var sut = MockRepository.GeneratePartialMock<ConfigurationInfoHandler>();

			var raveDataSettingsObject = _fixture.Create<object>();
			sut.Stub(x => x.GetRaveDataSettingsSectionObject()).Return(raveDataSettingsObject);

			dynamic connectionSetting1 = new ExpandoObject();
			connectionSetting1.ConnectionString = _fixture.Create<string>();
			connectionSetting1.DataSourceHint = "RaveDB";
			dynamic connectionSetting2 = new ExpandoObject();
			connectionSetting2.ConnectionString = _fixture.Create<string>();
			connectionSetting2.DataSourceHint = "RaveDB_Reporting";


			dynamic expendo = _fixture.Create<ExpandoObject>();
			expendo.ConnectionSettings = new List<object> { connectionSetting1, connectionSetting2 };

			dynamic defaultHint = _fixture.Create<ExpandoObject>();
			defaultHint.Value = "RaveDB";
			expendo.DefaultHint = defaultHint;
			sut.Stub(x => x.ConvertToExpendoObject(raveDataSettingsObject)).Return(expendo);

			var conn = _fixture.Create<IDbConnection>();
			sut.Stub(x => x.CreateConnection(null)).IgnoreArguments().Return(conn);

			var reader = _fixture.Create<IDataReader>();
			sut.Stub(x => x.GetDataReaderByConnection(null)).IgnoreArguments().Return(reader);

			List<object> list = new List<object>();
			sut.Stub(x => x.GetConfigurationInfoFromDb(reader)).IgnoreArguments().Return(list);

			//Act
			dynamic result = sut.Handler(question);
			//Assert
			//Assert
			Assert.AreSame(result, list);
			Assert.AreEqual(0, result.Count);
		}
	}
}
