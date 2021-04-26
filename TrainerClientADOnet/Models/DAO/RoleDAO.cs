using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Models.DAO
{
    public class RoleDAO
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public List<RoleDTO> GetAll()
        {
            List<RoleDTO> roles;
            using (SqlConnection connection = EstablishingConnectionUserRole.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from AspNetRoles", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    roles = new List<RoleDTO>();

                    while (reader.Read())
                    {
                        RoleDTO role = new RoleDTO
                        {
                            Id = (string)reader["Id"],
                            Name = (string)reader["Name"],
                        };
                        roles.Add(role);
                    }
                }
                return roles;
            }
        }

        //OK
        public void Create(string Name)
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(Name));
        }

        public void Delete(string id)
        {
            var role = context.Roles.Where(d => d.Id.Equals(id)).FirstOrDefault();
            context.Roles.Remove(role);
            context.SaveChanges();
        }
    }
}