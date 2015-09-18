using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.DBInfo
{
	[XmlRoot(ElementName = "DefaultHint")]
	public sealed class DefaultHint
	{
		[XmlAttribute(AttributeName = "Value")]
		public string Value { get; set; }
	}
}