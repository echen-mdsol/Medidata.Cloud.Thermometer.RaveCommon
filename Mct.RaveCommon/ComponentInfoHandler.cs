using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Hanlder that returns the component information.
    /// </summary>
    public class ComponentInfoHandler : ThermometerBaseHandler
    {
        public ComponentInfoHandler()
        {
            RaveComponentNames = new[]
            {
                "Medidata.Core.Service",
                "Medidata.Rave.Integration.Service",
                "Medidata.RaveWebServices.Web",
                "MedidataRAVE",
                "RaveCrystalViewer"
            };
        }

        protected internal virtual IEnumerable<string> RaveComponentNames { get; private set; }

        protected Assembly GetComponentAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.FirstOrDefault(a => RaveComponentNames.Contains(a.GetName().Name));

            return assembly;
        }

        [ExcludeFromCodeCoverage]
        internal protected string GetAssemblyAttributeFirstPropertyValue<T>(Assembly assembly) where T : Attribute
        {
            var type = typeof(T);
            var p = type.GetProperties().FirstOrDefault(pi => pi.Name != "TypeId");
            var attribute = assembly.GetCustomAttributes(type, false).OfType<T>().SingleOrDefault();
            return attribute == null || p == null ? string.Empty : p.GetValue(attribute, null).ToString();
        }

        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var componentAssembly = GetComponentAssembly();

            if (componentAssembly == null)
            {
                return "Unknown Component";
            }

            return new
            {
                product = GetAssemblyAttributeFirstPropertyValue<AssemblyProductAttribute>(componentAssembly),
                component = componentAssembly.GetName().Name,
                productVersion = GetAssemblyAttributeFirstPropertyValue<AssemblyFileVersionAttribute>(componentAssembly),
                buildId = GetAssemblyAttributeFirstPropertyValue<AssemblyInformationalVersionAttribute>(componentAssembly)
            };
        }
    }
}