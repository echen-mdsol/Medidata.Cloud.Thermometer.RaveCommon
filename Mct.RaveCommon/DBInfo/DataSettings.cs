using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.DBInfo
{
	[XmlRoot(ElementName = "DataSettings")]
	public sealed class DataSettings
	{
		[XmlElement(ElementName = "Caching")]
		public Caching Caching { get; set; }
		[XmlElement(ElementName = "DefaultHint")]
		public DefaultHint DefaultHint { get; set; }
		[XmlElement(ElementName = "ConnectionSettings")]
		public ConnectionSettings ConnectionSettings { get; set; }
	}
}