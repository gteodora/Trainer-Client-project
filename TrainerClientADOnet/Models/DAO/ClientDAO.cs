using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DAO
{
    public class ClientDAO:Controller
    {

         //OK
        public List<ClientDTO> GetAll()
        {
            List<ClientDTO> clients;
            using (SqlConnection connection =  EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from client", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    clients = new List<ClientDTO>();

                    while (reader.Read())
                    {
                        ClientDTO client = new ClientDTO
                        {
                            Id = (string)reader["id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                            RegistrationDate = Convert.ToDateTime(reader["registration_date"]),
                            Description = (string)reader["description"]
                        };
                        clients.Add(client);
                    }
                }
            return clients;
            }
        }

        //OK
        public string Create(string firstName, string lastName, DateTime dateOfBirth, DateTime registrationDate, string description)
        {
            // int id = 0;
            string id;
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("insert into client  ( first_name, last_name, date_of_birth, registration_date, description)values( @first_name, @last_name, @date_of_birth, @registration_date, @description)", connection))
                {
                    command.Parameters.AddWithValue("@first_name", firstName);
                    command.Parameters.AddWithValue("@last_name", lastName);
                    command.Parameters.AddWithValue("@date_of_birth", dateOfBirth);
                    command.Parameters.AddWithValue("@registration_date", registrationDate);
                    command.Parameters.AddWithValue("@description", description);
                    // command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar)).Direction = System.Data.ParameterDirection.Output;

                    command.ExecuteNonQuery();
                     id = (string)command.Parameters["@id"].Value;
                }
            }
            return id;
        }

        //OK
        public async Task<ActionResult> Create(string firstName, string lastName, DateTime dateOfBirth, DateTime registrationDate, string roleId, string email, string username, string password, bool emailConfirmed, bool phoneNumberConfirmed, bool twoFactorEnabled, bool lockoutEnabled, int accessFailedCount, string description = " ")
        {
            string id;
            var db = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new ApplicationUserManager(userStore);

            var user = new ApplicationUser { UserName = username, Email = email, EmailConfirmed = emailConfirmed, PhoneNumberConfirmed = phoneNumberConfirmed, TwoFactorEnabled = twoFactorEnabled, LockoutEnabled = lockoutEnabled, AccessFailedCount = accessFailedCount };
            if (user != null)
            {
                var result = await userManager.CreateAsync(user, password);
                id = user.Id;
                if (result.Succeeded)
                {
                    result = userManager.AddToRole(user.Id, Utility.CLIENT_ROLE); 
                    SendEmailWithCredentials(email, password); //todo: posalji e-mail ulogovanom korisniku

                    //Dodavanje u moju bazu korisnika sa ZADANIM id-jem:
                    /*using (SqlConnection connection = EstablishingConnection.GetConnection())
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SET IDENTITY_INSERT client ON " +
                            "insert into client  (id, first_name, last_name, date_of_birth, registration_date, description)values( @id, @first_name, @last_name, @date_of_birth, @registration_date, @description " +
                            "SET IDENTITY_INSERT client OFF)", connection))*/
                    using (SqlConnection connection = EstablishingConnection.GetConnection())
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("insert into client  ( id, first_name, last_name, date_of_birth, registration_date, [description])values( @id, @first_name, @last_name, @date_of_birth, @registration_date, @description)", connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@first_name", firstName);
                            command.Parameters.AddWithValue("@last_name", lastName);
                            command.Parameters.AddWithValue("@date_of_birth", dateOfBirth);
                            command.Parameters.AddWithValue("@registration_date", registrationDate);
                            command.Parameters.AddWithValue("@description", description);
                           // command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar)).Direction = System.Data.ParameterDirection.Output;

                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    return RedirectToAction("Index", "Client");
                }
                else AddErrors(result);
            }
            return View("Error");
        }

        private void SendEmailWithCredentials(string email, string password)
        {
            
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public List<ProgramDTO> GetAllPrograms(int clientId)
        {
            List<ProgramDTO> programs;
            string queryString = 
                "SELECT * from program" +
                    " WHERE program.client_id = @client_id";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@client_id", clientId);

                    SqlDataReader reader = command.ExecuteReader();
                    programs = new List<ProgramDTO>();

                    while (reader.Read())
                    {
                        ProgramDTO program = new ProgramDTO
                        {
                            Id = (int)reader["id"],
                            ClientId = (string)reader["client_id"],
                            GoalTypeId = (int)reader["goal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"]
                        };
                        programs.Add(program);
                    }
                    connection.Close();
                }
                return programs;
            }
        }

        public int Edit(string id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string description)
        {
            int numberOfAffectedRows;
            string query =
                "UPDATE client " +
                "SET first_name=@firstName, last_name=@lastName, date_of_birth=@birthDate, registration_date=@registrationDate, description=@description  " +
                "WHERE id = @id;";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@birthDate", birthDate);
                    command.Parameters.AddWithValue("@registrationDate", registrationDate);
                    command.Parameters.AddWithValue("@description", description);
                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                return numberOfAffectedRows;
            }
        }

        public List<ProgramDTO> GetAllPrograms(string clientId)
        {
            List<ProgramDTO> programs;
            string queryString =
                "SELECT * from program" +
                    " WHERE program.client_id = @client_id";

            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@client_id", clientId);
                    SqlDataReader reader = command.ExecuteReader();
                    programs = new List<ProgramDTO>();
                    while (reader.Read())
                    {
                        ProgramDTO program = new ProgramDTO
                        {
                            Id = (int)reader["id"],
                            ClientId = (string)reader["client_id"],
                            GoalTypeId = (int)reader["goal_type_id"],
                            StartTime = Convert.ToDateTime(reader["start_time"]),
                            EndTime = Convert.ToDateTime(reader["end_time"]),
                            Description = (string)reader["description"]
                        };
                        programs.Add(program);
                    }
                    connection.Close();
                }
                return programs;
            }
        }

        public void Delete(string id)
        {
            string query = "DELETE FROM client WHERE id=@id";
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


        public string getClientFullNameByProgramId(int programId)
        {
            ClientDTO c;
            string queryString =
            "SELECT c.first_name, c.last_name FROM client c JOIN program p ON p.client_id=c.id " +
            "WHERE p.id=@programId";
            using (SqlConnection connection = EstablishingConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@programId", programId);

                    SqlDataReader reader = command.ExecuteReader();
                    c = new ClientDTO();

                    while (reader.Read())
                    {
                        ClientDTO clientNew = new ClientDTO
                        {
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"]
                        };

                        c = clientNew;

                    }
                    connection.Close();
                }
                return c.FullName;
            }
        }

    }
}