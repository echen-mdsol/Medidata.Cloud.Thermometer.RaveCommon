using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Pull info from Configuration Database table
    /// </summary>
    public class ConfigurationInfoHandler : ThermometerBaseHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            try
            {
                var section = GetRaveDataSettingsSectionObject();

                dynamic expando = ConvertToExpendoObject(section);
                string defaultHint = expando.DefaultHint.Value;
                var connections = (List<dynamic>) expando.ConnectionSettings;
                var conn = connections.First(c => c.DataSourceHint == defaultHint);
                var connString = (string) conn.ConnectionString;
                return GetConfigurationInfoFromDb(GetDataReaderByConnection(connString));
            }
            catch (Exception e)
            {
                return e;
            }
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

        [ExcludeFromCodeCoverage]
        internal virtual IDataReader GetDataReaderByConnection(string connectionString)
        {
            var sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select Id,Tag,ConfigValue from [dbo].[Configuration]";
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                sqlConnection.Dispose();
            }
            
        }

        internal virtual List<object> GetConfigurationInfoFromDb(IDataReader dataReader)
        {
            var result = new List<object>();
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader", "DataReader should not be null");
            }
            using (IDataReader reader = dataReader)
            {
                while (reader.Read())
                {
                    result.Add(new { ID = reader["ID"], Tag = reader["Tag"], ConfigValue = reader["ConfigValue"] });
                }
                return result;
            }
            
            
        }
    }
}