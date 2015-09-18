using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.DBInfo
{
	[XmlRoot(ElementName = "Caching")]
	public sealed class Caching
	{
		[XmlAttribute(AttributeName = "TimeOut")]
		public int TimeOut { get; set; }
	}
}
