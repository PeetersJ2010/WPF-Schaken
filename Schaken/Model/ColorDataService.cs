using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaken.Model
{
    class ColorDataService
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;

        //Connection
        private static IDbConnection db = new SqlConnection(connectionString);

        public List<Color> GetColors()
        {
            //SQL Query
            string sql = "select * from Color order by ID";
            //Execute
            return (List<Color>)db.Query<Color>(sql);
        }
    }
}
