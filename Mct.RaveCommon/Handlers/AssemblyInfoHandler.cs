using System;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class AssemblyInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var assemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName());

            return assemblyNames.Where(an => an.Name.Contains("Medidata"))
                .ToDictionary(an => an.Name, an => an.Version.ToString());
        }
    }
}