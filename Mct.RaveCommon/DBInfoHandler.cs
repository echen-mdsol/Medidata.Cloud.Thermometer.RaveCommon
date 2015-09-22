using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class DbInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var dataSettings = GetRaveDataSettingsSectionObject();
            dynamic expando = ConvertToExpendoObject(dataSettings);
            var connectionSettings = (IList<object>) expando.ConnectionSettings;
            foreach (dynamic x in connectionSettings)
            {
                x.ConnectionState = GetConnectionState(x.ConnectionString);
                x.ConnectionString = ConvertConnectionStringToObject(x.ConnectionString);
            }

            return expando;
        }

        [ExcludeFromCodeCoverage]
        internal virtual object GetRaveDataSettingsSectionObject()
        {
            return ConfigurationManager.GetSection("DataSettings");
        }

        internal virtual ExpandoObject ConvertToExpendoObject(object target)
        {
            var expando = target as ExpandoObject;
            if (expando != null) return expando;
            var json = JsonConvert.SerializeObject(target, new StringEnumConverter());
            expando = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
            return expando;
        }

        internal virtual ExpandoObject ConvertConnectionStringToObject(string connectionString)
        {
            var connectionInfo = new SqlConnectionStringBuilder(connectionString);
            var obj = ConvertToExpendoObject(connectionInfo);
            IDictionary<string, object> dic = obj;
            dic.Remove("Password");
            dic.Remove("User ID");
            return obj;
        }

        internal virtual IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        internal virtual object GetConnectionState(string connString)
        {
            try
            {
                using (var conn = CreateConnection(connString))
                {
                    conn.Open();
                    return conn.State;
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}