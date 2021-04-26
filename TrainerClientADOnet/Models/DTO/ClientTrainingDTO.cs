using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class ClientTrainingDTO : TrainingDTO
    {
        //training-sve
        /*
        [DisplayName("Training id")]
        public int Id { get; set; }

        [DisplayName("Program id")]
        [Required]
        public int ProgramId { get; set; }

        [DisplayName("Training Type")]
        [Required(ErrorMessage = "Choose training type.")]
        public int TrainingTypeId { get; set; }

        [DisplayName("Start time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter start time.")]
        public DateTime StartTime { get; set; }

        [DisplayName("End time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter end time.")]
        public DateTime EndTime { get; set; }
        

        [DisplayName("Description")]
        //[Required] //nije obavezno !!!
        public string Description { get; set; }
        */


        //klijent
        [DisplayName("Client")]
        public string ClientId { get; set; }

        [DisplayName("First name")]
       // [Required]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
       // [Required]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}