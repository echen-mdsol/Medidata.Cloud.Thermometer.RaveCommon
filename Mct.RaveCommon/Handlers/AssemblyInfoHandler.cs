using System;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon.Handlers
{
    public class AssemblyInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var assemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName());

            return assemblyNames.GroupBy(an => an.Name)
                .ToDictionary(g => g.Key, g => g.First().Version.ToString());
        }
    }
}