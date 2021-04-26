using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class TrainingTypeDTO
    {
        [DisplayName("Training type id")]
        public int Id { get; set; }


        [DisplayName("Training type name")]
        [Required]
        public string Name { get; set; }
    }
}