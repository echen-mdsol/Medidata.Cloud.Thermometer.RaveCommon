using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Medidata.Cloud.Thermometer.RaveCommon.Handlers
{
    public class ComponentInfoHandler : ThermometerBaseHandler
    {
        private readonly List<string> _raveComponentNames = new List<string>
        {
            "Medidata.Core.Service",
            "Medidata.Rave.Integration.Service",
            "Medidata.RaveWebServices.Web",
            "MedidataRAVE",
            "RaveCrystalViewer"
        };

        protected virtual List<string> RaveComponentNames
        {
            get { return _raveComponentNames; }
        }

        [ExcludeFromCodeCoverage]
        protected Assembly GetComponentAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.FirstOrDefault(a => RaveComponentNames.Contains(a.GetName().Name));

            return assembly;
        }

        [ExcludeFromCodeCoverage]
        protected string GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            var attributes = assembly.GetCustomAttributes(typeof (T), false);
            var type = typeof (T);
            var p = type.GetProperties().FirstOrDefault(pi => pi.Name != "TypeId");

            if (attributes.Length == 0 || p == null)
            {
                return "";
            }

            var attribute = attributes.OfType<T>().SingleOrDefault();

            return p.GetValue(attribute, null).ToString();
        }

        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var componentAssembly = GetComponentAssembly();

            if (componentAssembly == null)
            {
                return "Unknown Component";
            }

            var componentAssemblyName = componentAssembly.GetName();

            return new
            {
                product = GetAssemblyAttribute<AssemblyProductAttribute>(componentAssembly),
                component = componentAssemblyName.Name,
                productVersion = GetAssemblyAttribute<AssemblyFileVersionAttribute>(componentAssembly),
                buildId = GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(componentAssembly)
            };
        }
    }
}