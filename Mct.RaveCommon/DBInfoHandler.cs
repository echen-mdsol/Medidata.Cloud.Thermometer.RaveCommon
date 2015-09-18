using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Medidata.Cloud.Thermometer.RaveCommon.DBInfo;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
	public class DBInfoHandler : ThermometerBaseHandler
	{

		protected override object HandleQuestion(IThermometerQuestion question)
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var dataSettingSection = config.Sections["DataSettings"];
			if (dataSettingSection == null)
			{
				return new {Error = "DataSettings section cannot be found"};
			}
			var rawXml = dataSettingSection.SectionInformation.GetRawXml();
			var serializer = new XmlSerializer(typeof(DataSettings));
			using (TextReader reader = new StringReader(rawXml))
			{
				var result = (DataSettings)serializer.Deserialize(reader);
				result.ConnectionSettings.ConnectionSetting.ForEach(e => e.FillConnectionStringDetails());
				return
					new
					{
						Caching = result.Caching,
						DefaultHint = result.DefaultHint,
						ConnectionSettings =
							result.ConnectionSettings.ConnectionSetting.Select(
								c =>
									new
									{
										DataSourceHint = c.DataSourceHint,
										ServerName = c.ServerName,
										DatabaseName = c.DatabaseName,
										ProviderType = c.ProviderType,
										MaxPoolSize = c.MaxPoolSize,
										MultipleActiveResultSets = c.MultipleActiveResultSets,
										ConnectionTimeOut = c.ConnectionTimeOut
									}).ToList()
					};
			}	
		}
	}
}
