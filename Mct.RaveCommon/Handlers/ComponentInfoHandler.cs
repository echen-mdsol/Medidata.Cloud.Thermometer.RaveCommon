using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Medidata.Cloud.Thermometer.RaveCommon.Handlers
{
    public class ComponentInfoHandler : ThermometerBaseHandler
    {
        private readonly List<string> raveComponentNames = new List<string>()
        {
            "Medidata.Core.Service",
            "Medidata.Rave.Integration.Service",
            "Medidata.RaveWebServices.Web",
            "MedidataRAVE",
            "RaveCrystalViewer"
        };

        private Assembly GetComponentAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (raveComponentNames.Contains(assembly.GetName().Name))
                {
                    return assembly;
                }
            }

            return assemblies.First();
        }

        public T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            var attributes = assembly.GetCustomAttributes(typeof(T), false);
            if (attributes.Length == 0)
            {
                return null;
            }
            return attributes.OfType<T>().SingleOrDefault();
        }

        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var componentAssembly = GetComponentAssembly();
            var componentAssemblyName = componentAssembly.GetName();

            return new
            {
                product = GetAssemblyAttribute<AssemblyProductAttribute>(componentAssembly).Product,
                component = componentAssemblyName.Name,
                productVersion = GetAssemblyAttribute<AssemblyFileVersionAttribute>(componentAssembly).Version,
                buildId = GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(componentAssembly).InformationalVersion,
            };
        }
    }
}