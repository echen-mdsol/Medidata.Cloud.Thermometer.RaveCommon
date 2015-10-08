using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    internal static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object target)
        {
            if (target == null) return null;

            var expandoObject = target as ExpandoObject;
            if (expandoObject != null) return expandoObject;

            var expando = new ExpandoObject();
            var dic = (IDictionary<string, object>) expando;
            var properties = TypeDescriptor.GetProperties(target.GetType()).OfType<PropertyDescriptor>();
            foreach (var property in properties)
            {
                dic.Add(property.Name, property.GetValue(target));
            }

            return expando;
        }
    }
}