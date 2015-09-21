using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests
{
	[TestClass]
	public class DBInfoHandlerTests
	{
		private IFixture _fixture;

		[TestInitialize]
		public void Init()
		{
			_fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
		}
		[TestMethod]
		public void HandlerShouldParseDataSettingsSection()
		{
			//Arrange
			var question = _fixture.Create<IThermometerQuestion>();

			//Act
			var sut = new DBInfoHandler();
			dynamic answer = sut.Handler(question);
			
			//Assert
			Assert.AreEqual(10, answer.CachingTimeOut);
			Assert.AreEqual("RaveDev", answer.DefaultHint);
			Assert.AreEqual(3, answer.ConnectionSettings.Count);
			Assert.AreEqual("RaveDev", answer.ConnectionSettings[0].DataSourceHint);
			Assert.AreEqual("RaveDev_Reporter", answer.ConnectionSettings[1].DataSourceHint);
			Assert.AreEqual("RaveDev_Migration", answer.ConnectionSettings[2].DataSourceHint);
		}
	}
}
