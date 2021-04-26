using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class ProgramGoalTypeDTO
    {
        [DisplayName("Program id")]
        public int Id { get; set; }

        [DisplayName("Client id")]
        [Required]
        public string ClientId { get; set; }

        [DisplayName("Goal Type")]
        [Required(ErrorMessage = "Choose goal type.")]
        public int GoalTypeId { get; set; }

        [DisplayName("Start time")]
        [DataType(DataType.Date)]
      //  [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter start time.")]
        public DateTime StartTime { get; set; }

        [DisplayName("End time")]
        [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Choose program type.")]
        public bool IsMeal { get; set; }

        //GoalType
        [DisplayName("Goal type")]
        public string Name { get; set; }

        [DisplayName("Program type")]
        public string ProgramType { get; set; }  //u zavisnoti kako je bool setovan, to ce i pisati

    }
}