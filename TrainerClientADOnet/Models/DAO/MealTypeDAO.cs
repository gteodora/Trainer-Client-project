using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Models.DAO
{
    public class MealTypeDAO
    {
        public List<MealTypeDTO> GetAll()
        {
            List<MealTypeDTO> mealTypes;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from meal_type", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    mealTypes = new List<MealTypeDTO>();

                    while (reader.Read())
                    {
                        MealTypeDTO client = new MealTypeDTO
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                        };
                        mealTypes.Add(client);
                    }
                }
                return mealTypes;
            }
        }

        public int Create(string name)
        {
            int id = 0;
            
            string query = "IF NOT EXISTS (SELECT * FROM meal_type d WHERE name = @name)" +
                "INSERT INTO meal_type (name) values (@name)";
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

        public MealTypeDTO getById(int Id)
        {
            MealTypeDTO mealTypeDTO;
            string query =
                "SELECT * FROM meal_type gt WHERE gt.id=@id ";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = command.ExecuteReader();
                    mealTypeDTO = new MealTypeDTO();

                    while (reader.Read())
                    {
                        MealTypeDTO newMealTypeDTO = new MealTypeDTO()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        };
                        mealTypeDTO = newMealTypeDTO;
                    }
                }
                connection.Close();
            }
            return mealTypeDTO;
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM meal_type WHERE id=@id";
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
                "UPDATE meal_type " +
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