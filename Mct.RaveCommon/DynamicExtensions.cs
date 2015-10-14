using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    internal static class DynamicExtensions
    {
        public static dynamic ToDynamicJson(this object target)
        {
            if (target == null) return null;

            var expandoObject = target as ExpandoObject;
            if (expandoObject != null) return expandoObject;

            var json = JsonConvert.SerializeObject(target, new StringEnumConverter());
            var obj = JsonConvert.DeserializeObject<ExpandoObject>(json,  new ExpandoObjectConverter());

            return obj;
        }
    }
}