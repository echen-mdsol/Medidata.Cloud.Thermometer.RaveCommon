using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;

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
				return new {Error = "DataSettings section could not be found"};
			}
			var rawXml = dataSettingSection.SectionInformation.GetRawXml();
			var xDoc = new XmlDocument();
			xDoc.LoadXml(rawXml);
			var timeOut = int.Parse(xDoc.SelectSingleNode("//DataSettings/Caching").Attributes["TimeOut"].Value);
			var defaultHint = xDoc.SelectSingleNode("//DataSettings/DefaultHint").Attributes["Value"].Value;
			var connList =
				xDoc.SelectNodes("//DataSettings/ConnectionSettings/ConnectionSetting")
					.OfType<XmlNode>()
					.Select(GenerateConnectionStringObject)
					.ToList();
			return new {CachingTimeOut = timeOut, DefaultHint = defaultHint, ConnectionSettings = connList};
		}

		private object GenerateConnectionStringObject(XmlNode node)
		{
			var hint = node.Attributes["DataSourceHint"].Value;
			var connString = node.Attributes["ConnectionString"].Value;
			var providerType = node.Attributes["ProviderType"].Value;
			var sb = new SqlConnectionStringBuilder(connString);
			return new
			{
				DataSourceHint = hint,
				ServerName = sb.DataSource,
				DatabaseName = sb.InitialCatalog,
				ProviderType = providerType,
				sb.MaxPoolSize,
				sb.MultipleActiveResultSets,
				ConnectionTimeOut = sb.ConnectTimeout,
				CanConnect = TryConnectionString(connString)
			};
		}

		private object TryConnectionString(string connString)
		{
			using (var conn = new SqlConnection(connString))
			{
				try
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{
						return "Yes";
					}
					return conn.State.ToString();
				}
				catch (Exception e)
				{
					return e;
				}
			}
		}
	}
}