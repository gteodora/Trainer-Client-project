using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DAO
{
    public class MealDAO
    {
        public List<MealDTO> getByProgram(int programId, DateTime startTime, DateTime endTime)
        {
            List<MealDTO> mealsInProgram;
            string queryString =
                "SELECT * from meal" +
                    " WHERE  @program_id = meal.program_id AND" +
                    " start_time between @monday and @sunday";


            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@program_id", programId);

                    var monday = startTime.Date;// GetThisWeekMonday().Date;
                    var sunday = Utility.ChangeTime(endTime, 22, 0, 0);
                    command.Parameters.AddWithValue("@monday", monday);
                    command.Parameters.AddWithValue("@sunday", sunday);

                    SqlDataReader reader = command.ExecuteReader();
                    mealsInProgram = new List<MealDTO>();

                    while (reader.Read())
                    {
                        MealDTO program = new MealDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            MealTypeId = (int)reader["meal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            Description = (string)reader["description"],
                        };
                        mealsInProgram.Add(program);
                    }
                    connection.Close();
                }
                return mealsInProgram;
            }
        }

        public MealDTO GetById(int id)
        {
            MealDTO meal;
            string queryString =
                "SELECT * from meal" +
                    " WHERE  @meal_id = meal.id";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@meal_id", id);

                    SqlDataReader reader = command.ExecuteReader();
                    meal = new MealDTO();

                    while (reader.Read())
                    {
                        MealDTO mealNew = new MealDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            MealTypeId = (int)reader["meal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            Description = (string)reader["description"],
                        };
                        meal = mealNew;
                    }
                    connection.Close();
                }
                return meal;
            }
        }

        internal int Create(int programId, int mealTypeId, DateTime date, string description)
        {
            int id = 0;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("insert into meal  ( program_id, meal_type_id, start_time, description)values( @program_id, @meal_type_id, @start_time, @description)", connection))
                {
                    command.Parameters.AddWithValue("@program_id", programId);
                    command.Parameters.AddWithValue("@meal_type_id", mealTypeId);
                    command.Parameters.AddWithValue("@start_time", date);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;

                    //  command.ExecuteNonQuery();
                    // id = (int)command.Parameters["@id"].Value;
                    // id = Convert.ToInt32(command.Parameters["@id"].Value);
                    id = Convert.ToInt32(command.ExecuteScalar());   //Ovo mi ne vrati NULL od insertovanog reda, jer ovo izvrs ikomandu i onda vrati prvi red iy prve insertovane kolone
                }
            }
            return id;
        }

        public int Edit(int id, string description)
        {
            int numberOfAffectedRows;
            string queryString =
                "UPDATE meal " +
                "SET meal.description=@description_meal " +
                "WHERE meal.id = @meal_id;";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@meal_id", id);
                    command.Parameters.AddWithValue("@description_meal", description);

                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                return numberOfAffectedRows;
            }
        }


    }
}