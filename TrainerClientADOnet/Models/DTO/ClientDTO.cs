using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DTO
{
    public class ClientDTO
    {
        [DisplayName("Client id")]
        public string Id { get; set; }

        [DisplayName("First name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        [ValidateBirthnDate(-10)]
        //  [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        [DisplayName("Registration date")]
        [Required]
        
        [DataType(DataType.Date)]
      //  [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }
    }
}