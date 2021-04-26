using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DAO
{
    public class ProgramGoalTypeDAO
    {
        public List<ProgramGoalTypeDTO> GetAllOfClient(string clientId)
        {
            List<ProgramGoalTypeDTO> programs;
            string queryString =
            "SELECT p.id as id_program, p.client_id as client_id, p.start_time as start_time, p.end_time as end_time, p.goal_type_id as goal_type_id,  g.id, p.is_meal as is_meal, g.name as name, p.description as description FROM program p JOIN goal_type g ON goal_type_id = g.id" +
                " WHERE @client_id = client_id";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@client_id", clientId);

                    SqlDataReader reader = command.ExecuteReader();
                    programs = new List<ProgramGoalTypeDTO>();

                    while (reader.Read())
                    {
                        ProgramGoalTypeDTO program = new ProgramGoalTypeDTO
                        {
                            Id = (int)reader["id_program"],
                            ClientId = (string)reader["client_id"],
                            GoalTypeId = (int)reader["goal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                            IsMeal = (bool)reader["is_meal"],
                            Name= (string)reader["name"]
                        };
                        program.ProgramType = Utility.setProgramType(program.IsMeal);

                        programs.Add(program);
                        
                    }
                    connection.Close();
                }
                return programs;
            }
        }

        //****

        public ProgramGoalTypeDTO GetById(int Id)
        {
            ProgramGoalTypeDTO program;
            string queryString =
            "SELECT p.id as id_program, p.client_id as client_id, p.start_time as start_time, p.end_time as end_time, p.goal_type_id as goal_type_id,  g.id, p.is_meal as is_meal, g.name as name, p.description as description FROM program p JOIN goal_type g ON goal_type_id = g.id" +
                " WHERE @id = p.id";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = command.ExecuteReader();
                    program = new ProgramGoalTypeDTO();

                    while (reader.Read())
                    {
                        ProgramGoalTypeDTO programNew = new ProgramGoalTypeDTO
                        {
                            Id = (int)reader["id_program"],
                            ClientId = (string)reader["client_id"],
                            GoalTypeId = (int)reader["goal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"],
                            IsMeal = (bool)reader["is_meal"],
                            Name = (string)reader["name"]
                        };
                        program.ProgramType= Utility.setProgramType(program.IsMeal);

                        program = programNew;

                    }
                    connection.Close();
                }
                return program;
            }

        }
    }
}

/*
 * "SELECT p.id, p.client_id as client_id, p.start_time as start_time, p.end_time as end_time, p.goal_type_id as goal_type_id, p.description as description, p.is_meal as is_meal, g.name as name  FROM program p JOIN goal_type g ON  goal_type.id = program.goal_type_id" +
                    " WHERE @client_id = program.client_id";
*/