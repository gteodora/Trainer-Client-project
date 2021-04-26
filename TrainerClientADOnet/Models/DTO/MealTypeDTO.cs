using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Models.DTO
{
    public class MealTypeDTO
    {
        [DisplayName("Meal type id")]
        public int Id { get; set; }

        [DisplayName("Meal type name")]
        [Required]
        public string Name { get; set; }
    }
}
