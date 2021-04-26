using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Models.DAO
{
    public class ProgramDAO
    {
        public int Create(string clientId, int goalTypeId, DateTime startTime, DateTime endTime, string description, bool isMeal) 
        {
            int result = 0;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreateProgram", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@goalTypeId", goalTypeId);
                    command.Parameters.AddWithValue("@startTime", startTime);
                    command.Parameters.AddWithValue("@endTime", endTime);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@isMeal", isMeal);
                    command.Parameters.Add(new SqlParameter("@lastId", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    // result = Convert.ToInt32(command.ExecuteScalar());  //vrati nulu,ali doro upise u bazu  -->rijeseno sa select scope
                    result = (int)command.Parameters["@lastId"].Value;
                }
            }
            return result;
        }

        internal int Edit(int id, string name)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM program WHERE id=@id";
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

        internal ProgramDTO getById(string programId)
        {
            ProgramDTO program;
            string queryString =
            "SELECT client_id FROM program " +
                " WHERE @id = id";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@id", programId);

                    SqlDataReader reader = command.ExecuteReader();
                    program = new ProgramDTO();

                    while (reader.Read())
                    {
                        ProgramDTO programNew = new ProgramDTO
                        {
                            ClientId = (string)reader["client_id"],
                        };

                        program = programNew;

                    }
                    connection.Close();
                }
                return program;
            }
        }

        public List<ProgramGoalTypeDTO> GetAllByClient(string clientId)
        {
            //string query = "";
            return null;
        }
    }
}

//gddhghgkdhdkghdkrhdggddgldjd jer mjut lkjui

