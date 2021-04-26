using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class UserRoleDTO
    {
        //userrole
        [DisplayName("User id")]
        public string Id { get; set; }

        [DisplayName("Role")]
        [Required]
        public string RoleId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }


        /*user*/
        /* [DisplayName("User id")]
         public string Id { get; set; }*/

        [DisplayName("Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DisplayName("EmailConfirmed")]
        [Required]
        public Boolean EmailConfirmed { get; set; }

        [DisplayName("PasswordHash")]
        public string PasswordHash { get; set; }

        [DisplayName("SecurityStamp")]
        public string SecurityStamp { get; set; }

        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DisplayName("PhoneNumberConfirmed")]
        [Required]
        public Boolean PhoneNumberConfirmed { get; set; }

        [DisplayName("TwoFactorEnabled")]
        [Required]
        public Boolean TwoFactorEnabled { get; set; }

        [DisplayName("LockoutEndDateUtc")]
        public DateTime LockoutEndDateUtc { get; set; }

        [DisplayName("LockoutEnabled")]
        [Required]
        public Boolean LockoutEnabled { get; set; }

        [DisplayName("AccessFailedCount")]
        [Required]
        public int AccessFailedCount { get; set; }

        [DisplayName("Username")]
        [Required]
        public string UserName { get; set; }


        /*role*/
        [DisplayName("Role name")]
       
        public string Name { get; set; }
    }
}