using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DTO
{
    public class ClientUserRoleDTO
    {
        [DisplayName("Client id")]
        public string Id { get; set; }

        [DisplayName("First name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required]
        public string LastName { get; set; }


        [DisplayName("Date of birth")]
        [ValidateBirthnDate(-10)]
        [Required]
        [DataType(DataType.Date)]
        //  [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        [DisplayName("Registration date")]
        [Required]
        [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }


        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }

        /////////////////////////////////////

        //userrole
       // [DisplayName("User id")]
       // public string Id { get; set; }

        [DisplayName("Role")]
      //  [Required]
        public string RoleId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required, MinLength(8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
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
      //  [Required]
        public string UserName { get; set; }

    }
}