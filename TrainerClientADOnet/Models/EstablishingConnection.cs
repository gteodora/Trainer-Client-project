using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models
{
    public class EstablishingConnection
    {
        private static string ConnectionString;

        static EstablishingConnection()
        {
            BuildConnectionString();
        }

        private static void BuildConnectionString()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConectionString"].ConnectionString;
            //using (SqlConnection connection = EstablishingConnection.GetConnection())
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}