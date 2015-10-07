using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class DbInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var dataSettings = GetRaveDataSettingsSectionObject();
            dynamic expando = dataSettings.ToDynamic();
            var connectionSettings = ((IEnumerable) expando.ConnectionSettings)
                                    .OfType<object>()
                                    .Select(x => x.ToDynamic());
            foreach (var x in connectionSettings)
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

        internal virtual dynamic ConvertConnectionStringToObject(string connectionString)
        {
            var connectionInfo = new SqlConnectionStringBuilder(connectionString);
            var obj = connectionInfo.ToDynamic();
            IDictionary<string, object> dic = obj;
            dic.Remove("Password");
            dic.Remove("UserID");
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