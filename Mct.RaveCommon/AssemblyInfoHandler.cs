using System;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class AssemblyInfoHandler : ThermometerBaseHandler
    {
        public AssemblyInfoHandler(string route, string name = null)
            : base(route, name)
        {
          
        }

        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var assemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName());

            return assemblyNames.Where(an => an.Name.Contains("Medidata"))
                .ToDictionary(an => an.Name, an => an.Version.ToString());
        }
    }
}