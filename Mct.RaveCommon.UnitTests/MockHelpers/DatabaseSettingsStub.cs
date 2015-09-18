using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.MockHelpers
{
	public class DatabaseSettingsStub:IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			object xmlSettings = null;
			XmlSerializer xmlSerializer = null;
			XmlNodeReader xmlReader = null;
			try
			{
				if (section != null)
				{
					XmlAttribute typeAttribute = GetTypeAttribute(section.Attributes);
					if (typeAttribute != null)
					{
						Type type = Type.GetType(typeAttribute.Value);
						if (type != null)
						{
							xmlSerializer = new XmlSerializer(type);
							xmlReader = new XmlNodeReader(section);
							xmlSettings = xmlSerializer.Deserialize(xmlReader);
						}
					}
				}
			}
			catch (XmlException)
			{
				xmlSettings = null;
			}
			catch (ConfigurationException)
			{
				xmlSettings = null;
			}
			finally
			{
				if (xmlReader != null) xmlReader.Close();
			}
			return xmlSettings;
		}
		private static XmlAttribute GetTypeAttribute(XmlAttributeCollection attributes)
		{
			XmlAttribute foundAttribute = null;
			for (int i = 0; foundAttribute == null && i < attributes.Count; i++)
			{
				XmlAttribute currentAttribute = attributes[i];
				if (String.Compare(currentAttribute.Name, "Type", true, CultureInfo.InvariantCulture) == 0) foundAttribute = currentAttribute;
			}
			return foundAttribute;
		}
	}
}
