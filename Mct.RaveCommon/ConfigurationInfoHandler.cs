using System.Data;
using System.Data.SqlClient;
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
            const string sql = "SELECT Tag, ConfigValue FROM [dbo].[Configuration] ORDER BY Id";
            var adapter = CreateDataAdapter(sql, connectionString);

            var set = new DataSet();
            adapter.Fill(set);

            return set.Tables[0];
        }

        internal virtual IDataAdapter CreateDataAdapter(string query, string connectionString)
        {
            return new SqlDataAdapter(query, connectionString);
        }

        internal virtual string GetConnectionString()
        {
            var dataSettings = GetRaveDataSettingsSectionObject();
            return dataSettings.ToDynamic().ConnectionSettings[0].ConnectionString;
        }
    }
}