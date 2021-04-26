using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Models.DAO
{
    public class TrainingTypeDAO
    {
        public int Create(string name)
        {
            int id = 0;
            string query = "IF NOT EXISTS (SELECT * FROM training_type d WHERE name = @name)" +
                "INSERT INTO training_type (name) values (@name)";
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
        public List<TrainingTypeDTO> GetAll()
        {
            List<TrainingTypeDTO> trainingTypes;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from training_type", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    trainingTypes = new List<TrainingTypeDTO>();

                    while (reader.Read())
                    {
                        TrainingTypeDTO trainingType = new TrainingTypeDTO
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                        };
                        trainingTypes.Add(trainingType);
                    }
                }
                return trainingTypes;
            }
        }

        public int Edit(int id,string name)
        {
            int numberOfAffectedRows;
            string query =
                "UPDATE training_type " +
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

        public TrainingTypeDTO getById(int Id)
        {
            TrainingTypeDTO trainingTypeDTO;
            string query =
                "SELECT * FROM training_type gt WHERE gt.id=@id ";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = command.ExecuteReader();
                    trainingTypeDTO = new TrainingTypeDTO();

                    while (reader.Read())
                    {
                        TrainingTypeDTO newTrainingTypeDTO = new TrainingTypeDTO()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        };
                        trainingTypeDTO = newTrainingTypeDTO;
                    }
                }
                connection.Close();
            }
            return trainingTypeDTO;
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM training_type WHERE id=@id";
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
    }
}