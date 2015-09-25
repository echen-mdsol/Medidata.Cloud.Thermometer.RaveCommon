using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
	/// <summary>
	/// Pull info from Configuration Database table
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
				var connections = (List<dynamic>)expando.ConnectionSettings;
				var conn = connections.First(c => c.DataSourceHint == defaultHint);
				var connString = (string)conn.ConnectionString;
			
				var connection = CreateConnection(connString);
				return GetConfigurationInfoFromDb(GetDataReaderByConnection(connection));
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

		internal virtual IDbConnection CreateConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
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
		internal virtual IDataReader GetDataReaderByConnection(IDbConnection connection)
		{
			using (SqlConnection sqlConnection = (SqlConnection)connection)
			{
				sqlConnection.Open();
				using (SqlCommand cmd = new SqlCommand("select Id,Tag,ConfigValue from [dbo].[Configuration]", sqlConnection))
				{
					return cmd.ExecuteReader();
				}
			}
		}

		internal virtual List<object> GetConfigurationInfoFromDb(IDataReader reader)
		{
			var result = new List<object>();
			while (reader.Read())
			{
				result.Add(new {ID = reader["ID"], Tag = reader["Tag"], ConfigValue = reader["ConfigValue"]});
			}
			return result;
		}
	}
	
}
