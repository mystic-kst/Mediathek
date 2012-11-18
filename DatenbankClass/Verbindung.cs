using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DatenbankClass
{
    public class Verbindung
    {
        public static SqlConnection GetSqlConnection()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
			//sb.DataSource = @"PC01100015\SQLEXPRESS";
			//sb.InitialCatalog = "MovieDatabase";
			//sb.IntegratedSecurity = true;

			string connectionString = ConfigurationManager.ConnectionStrings[2].ConnectionString;

	        return new SqlConnection(connectionString);
        }
    }
}
