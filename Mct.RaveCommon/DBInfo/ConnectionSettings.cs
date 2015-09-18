using System.Collections.Generic;
using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.DBInfo
{
	[XmlRoot(ElementName = "ConnectionSettings")]
	public sealed class ConnectionSettings
	{
		[XmlElement(ElementName = "ConnectionSetting")]
		public List<ConnectionSetting> ConnectionSetting { get; set; }
	}
}