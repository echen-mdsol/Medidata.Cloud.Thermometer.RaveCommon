using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Pull info from Configuration Database table
    /// </summary>
    public class ConfigurationInfoHandler : DbInfoHandler
    {
        protected override object HandleQuestion(IThermometerQuestion question)
        {
            var connectionString = GetConnectionString();
            const string sql = "SELECT Tag, ConfigValue FROM [dbo].[Configuration] ORDER BY Tag";
            var adapter = CreateDataAdapter(sql, connectionString);

            var set = new DataSet();
            adapter.Fill(set);

            var answer = FlattenToObject(set.Tables[0]);
            return answer;
        }

        internal virtual IDataAdapter CreateDataAdapter(string query, string connectionString)
        {
            return new SqlDataAdapter(query, connectionString);
        }

        internal virtual string GetConnectionString()
        {
            var dataSettings = GetRaveDataSettingsSectionObject();
            return dataSettings.ToDynamicJson().ConnectionSettings[0].ConnectionString;
        }

        internal virtual object FlattenToObject(DataTable table)
        {
            IDictionary<string, object> result = new ExpandoObject();
            var rows = table.Rows.OfType<DataRow>();
            foreach (var row in rows)
            {
                result.Add(row["Tag"] as string, row["ConfigValue"]);
            }

            return result;
        }
    }
}