using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.DBInfo
{
	[XmlRoot(ElementName = "ConnectionSetting")]
	public sealed class ConnectionSetting
	{
		[XmlAttribute(AttributeName = "DataSourceHint")]
		public string DataSourceHint { get; set; }
		[XmlAttribute(AttributeName = "ConnectionString")]
		public string ConnectionString { get; set; }
		[XmlAttribute(AttributeName = "ProviderType")]
		public string ProviderType { get; set; }

		[XmlIgnore]
		public string ServerName { get; set; }
		[XmlIgnore]
		public string DatabaseName { get; set; }

		[XmlIgnore]
		public bool MultipleActiveResultSets { get; set; }
		[XmlIgnore]
		public int ConnectionTimeOut { get; set; }
		[XmlIgnore]
		public int MaxPoolSize { get; set; }
		public void FillConnectionStringDetails()
		{
			SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(ConnectionString);
			ServerName = sb.DataSource;
			DatabaseName = sb.InitialCatalog;
			MultipleActiveResultSets = sb.MultipleActiveResultSets;
			ConnectionTimeOut = sb.ConnectTimeout;
		}
	}
}