using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DAO
{
    public class TrainingDAO
    {
        public List<TrainingDTO> getByProgram(int programId, DateTime startTime, DateTime endTime)
        {
            List<TrainingDTO> trainingsInProgram;
            string queryString =
                "SELECT * from training" +
                    " WHERE  @program_id = training.program_id AND" +
                    " start_time between @monday and @sunday";


            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@program_id", programId);

                    //var monday = Utility.GetPickedMonday().Date;// GetThisWeekMonday().Date;
                   // var sunday = Utility.ChangeTime(Utility.GetPickedSunday(), 22, 0, 0);
                    var monday = startTime.Date;// GetThisWeekMonday().Date;
                    var sunday = Utility.ChangeTime(endTime, 22, 0, 0);
                    command.Parameters.AddWithValue("@monday", monday);
                    command.Parameters.AddWithValue("@sunday", sunday);

                    SqlDataReader reader = command.ExecuteReader();
                    trainingsInProgram = new List<TrainingDTO>();

                    while (reader.Read())
                    {
                        TrainingDTO program = new TrainingDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                        };
                        trainingsInProgram.Add(program);
                    }
                    connection.Close();
                }
                return trainingsInProgram;
            }
        }

        public int Create(int programId, int trainingTypeId, DateTime startTime, object endTime, string description)
        {
            int id = 0;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("insert into training  ( program_id, training_type_id, start_time, end_time, description)values( @program_id, @training_type_id, @start_time, @end_time, @description)", connection))
                {
                    command.Parameters.AddWithValue("@program_id", programId);
                    command.Parameters.AddWithValue("@training_type_id", trainingTypeId);
                    command.Parameters.AddWithValue("@start_time", startTime);
                    command.Parameters.AddWithValue("@end_time", endTime);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;

                    id = Convert.ToInt32(command.ExecuteScalar()); 
                }
            }
            return id;
        }

        internal int CreateWithoutClientAndProgram(int trainingTypeId, DateTime startTime, DateTime endTime, string description)
        {
            throw new NotImplementedException();
        }

        public TrainingDTO GetById(int id)
        {
            TrainingDTO training;
            string queryString =
                "SELECT * from training" +
                    " WHERE  @training_id = training.id";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@training_id", id);

                    SqlDataReader reader = command.ExecuteReader();
                    training = new TrainingDTO();

                    while (reader.Read())
                    {
                        TrainingDTO trainingNew = new TrainingDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                        };
                        training = trainingNew;
                    }
                    connection.Close();
                }
                return training;
            }
        }

        public int Edit(int id, int trainingTypeId, DateTime startTime, DateTime endTime, string description)
        {
            int numberOfAffectedRows;
            string queryString =
                "UPDATE training " +
                "SET training.description=@description_training, training.training_type_id=@training_type_id, " +
                " training.start_time=@start_time, training.end_time=@end_time " +
                "WHERE training.id = @training_id;";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@training_type_id", trainingTypeId);
                    command.Parameters.AddWithValue("@start_time", startTime);
                    command.Parameters.AddWithValue("@end_time", endTime);
                    command.Parameters.AddWithValue("@training_id", id);
                    command.Parameters.AddWithValue("@description_training", description);

                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                return numberOfAffectedRows;
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM training WHERE id=@id";
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

        public void Edit(int id, int trainingTypeId, string description)
        {
            int numberOfAffectedRows;
            string queryString =
                "UPDATE training " +
                "SET training.description=@description_training, training.training_type_id=@training_type_id " +
                "WHERE training.id = @training_id;";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@training_type_id", trainingTypeId);
                    command.Parameters.AddWithValue("@training_id", id);
                    command.Parameters.AddWithValue("@description_training", description);

                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                
            }
        }

     
        internal List<TrainingDTO> getTrainingsByProgramDay(int programId, DateTime startDateTime)
        {
            List<TrainingDTO> trainingsInProgram;
            string queryString =
                "SELECT * from training" +
                    " WHERE  @program_id = training.program_id AND" +
                    " start_time between @startTime and @endTime";


            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@program_id", programId);

                   
                    var startTime = startDateTime.Date;// GetThisWeekMonday().Date;
                    var endTime = Utility.ChangeTime(startTime, 22, 0, 0);
                    command.Parameters.AddWithValue("@startTime", startTime);
                    command.Parameters.AddWithValue("@endTime", endTime);

                    SqlDataReader reader = command.ExecuteReader();
                    trainingsInProgram = new List<TrainingDTO>();

                    while (reader.Read())
                    {
                        TrainingDTO program = new TrainingDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                        };
                        trainingsInProgram.Add(program);
                    }
                    connection.Close();
                }
                return trainingsInProgram;
            }
        }

        public List<ClientTrainingDTO> getTrainerTrainingsInThisWeek()
        {
                List<ClientTrainingDTO> trainings;
            var training_type = 2;// todo Utility.TrainerTrainingType;
                string queryString =
                    "SELECT t.id as id, t.program_id as program_id, t.training_type_id as training_type_id, t.start_time, t.end_time, t.description as description, c.id as client_id, c.first_name as first_name, c.last_name as last_name  FROM training t " +
                    "JOIN program p ON p.id=program_id " +
                    "JOIN client c ON c.id=p.client_id " +
                        " WHERE  @training_type = training_type_id AND" +
                        " t.start_time between @monday and @sunday";


                using (SqlConnection connection = EstablishingConnection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@training_type", training_type);

                        var monday = Utility.GetThisWeekMonday().Date;
                        var sunday = Utility.ChangeTime(Utility.GetThisWeekSunday(), 22, 0, 0);
                        command.Parameters.AddWithValue("@monday", monday);
                        command.Parameters.AddWithValue("@sunday", sunday);

                        SqlDataReader reader = command.ExecuteReader();
                        trainings = new List<ClientTrainingDTO>();

                        while (reader.Read())
                        {
                        ClientTrainingDTO training = new ClientTrainingDTO
                        {
                                Id = (int)reader["id"], //training id
                                ProgramId = (int)reader["program_id"],
                                TrainingTypeId = (int)reader["training_type_id"],
                                StartTime = Convert.ToDateTime(reader["start_time"]),
                                EndTime = Convert.ToDateTime(reader["end_time"]),
                                Description = (string)reader["description"],

                                ClientId= (string)reader["client_id"],
                                FirstName = (string)reader["first_name"],
                                LastName = (string)reader["last_name"]
                        };
                            trainings.Add(training);
                        }
                        connection.Close();
                    }
                    return trainings;
                }
            }

        //vraca treninge koji imaju taj opis i tip  (taj su dan)
        public List<TrainingDTO> getTrainingsByDescriptionTypeDay(int programId, string description, DateTime startTime, int trainingTypeId)
        {
            List<TrainingDTO> trainingsInProgram;
            string queryString =
                "SELECT * from training" +
                    " WHERE  @description = training.description AND " +
                    "@trainingTypeId = training.training_type_id AND" +
                    " training.start_time between @startTimeMorning and @startTimeEvening AND " +
                    " training.program_id = @programId";

            // " start_time between @monday and @sunday";


            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@trainingTypeId", trainingTypeId);
                    command.Parameters.AddWithValue("@programId", programId);

                    var startTimeMorning = Utility.ChangeTime(startTime, 07, 0, 0);
                    var startTimeEvening = Utility.ChangeTime(startTime, 22, 0, 0);
                     command.Parameters.AddWithValue("@startTimeMorning", startTimeMorning);
                     command.Parameters.AddWithValue("@startTimeEvening", startTimeEvening);

                    SqlDataReader reader = command.ExecuteReader();
                    trainingsInProgram = new List<TrainingDTO>();

                    while (reader.Read())
                    {
                        TrainingDTO program = new TrainingDTO
                        {
                            Id = (int)reader["id"],
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                        };
                        trainingsInProgram.Add(program);
                    }
                    connection.Close();
                }
                return trainingsInProgram;
            }
        }


        public TrainingDTO getById(int Id)
        {
            TrainingDTO training;
            string query =
                "SELECT * FROM training WHERE id=@id ";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = command.ExecuteReader();
                    training = new TrainingDTO();

                    while (reader.Read())
                    {
                        TrainingDTO newTraining = new TrainingDTO()
                        {
                            Id = (int)reader["id"],
                            Description = (string)reader["description"],
                            StartTime=Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"]

                        };
                        training = newTraining;
                    }
                }
                connection.Close();
            }
            return training;
        }

        public List<ClientTrainingDTO> getTrainerTrainingsInPickedWeek(DateTime Monday)
        {
            List<ClientTrainingDTO> trainings;
            var training_type = 2;// todo Utility.TrainerTrainingType;
            string queryString =
                "SELECT t.id as id, t.program_id as program_id, t.training_type_id as training_type_id, t.start_time, t.end_time, t.description as description, c.id as client_id, c.first_name as first_name, c.last_name as last_name  FROM training t " +
                "JOIN program p ON p.id=program_id " +
                "JOIN client c ON c.id=p.client_id " +
                    " WHERE  @training_type = training_type_id AND" +
                    " t.start_time between @monday and @sunday";


            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@training_type", training_type);

                    var monday = Monday.Date;
                    var sunday = Monday.AddDays(6);
                    command.Parameters.AddWithValue("@monday", monday);
                    command.Parameters.AddWithValue("@sunday", sunday);

                    SqlDataReader reader = command.ExecuteReader();
                    trainings = new List<ClientTrainingDTO>();

                    while (reader.Read())
                    {
                        ClientTrainingDTO training = new ClientTrainingDTO
                        {
                            Id = (int)reader["id"], //training id
                            ProgramId = (int)reader["program_id"],
                            TrainingTypeId = (int)reader["training_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],

                            ClientId = (string)reader["client_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"]
                        };
                        trainings.Add(training);
                    }
                    connection.Close();
                }
                return trainings;
            }
        }
    }
}