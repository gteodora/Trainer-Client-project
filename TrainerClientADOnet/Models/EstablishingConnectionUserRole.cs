using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models
{
    public class EstablishingConnectionUserRole
    {
        private static string ConnectionString;

        static EstablishingConnectionUserRole()
        {
            BuildConnectionString();
        }

        private static void BuildConnectionString()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //using (SqlConnection connection = EstablishingConnectionUserRole.GetConnection())
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}