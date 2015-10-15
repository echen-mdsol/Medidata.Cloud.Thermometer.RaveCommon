using System.Linq;
using System.Web.Routing;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class RouteInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            return RouteTable.Routes.OfType<Route>().Select(x => x.Url);
        }
    }
}