using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Models.DAO
{
    public class GoalTypeDAO
    {
        public int Create(string name)
        {
            int id = 0;
            string query = "IF NOT EXISTS (SELECT * FROM goal_type d WHERE name = @name)" +
                "INSERT INTO goal_type (name) values (@name)";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    id = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
            return id;
        }
        public List<GoalTypeDTO> GetAll()
        {
            List<GoalTypeDTO> goalTypes;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from goal_type", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    goalTypes = new List<GoalTypeDTO>();

                    while (reader.Read())
                    {
                        GoalTypeDTO client = new GoalTypeDTO
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                        };
                     goalTypes.Add(client);
                    }
                }
                return goalTypes;
            }
        }

        public GoalTypeDTO getById(int Id)
        {
            GoalTypeDTO goalTypeDTO;
            string query =
                "SELECT * FROM goal_type gt WHERE gt.id=@id ";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command=new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = command.ExecuteReader();
                    goalTypeDTO = new GoalTypeDTO();

                    while (reader.Read())
                    {
                        GoalTypeDTO newGoalTypeDTO = new GoalTypeDTO()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        };
                        goalTypeDTO = newGoalTypeDTO;
                    }
                }
                connection.Close();
            }
            return goalTypeDTO;
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM goal_type WHERE id=@id";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    SqlCommand Command = new SqlCommand(query, connection);
                    Command.Parameters.AddWithValue("@id", id);
                    Command.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public int Edit(int id, string name)
        {
            int numberOfAffectedRows;
            string query =
                "UPDATE goal_type " +
                "SET name=@name WHERE id = @id;";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", name);

                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                return numberOfAffectedRows;
            }
        }
    }
}