//using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class MealDTO
    {
        private static int IdCounter=0;
        public MealDTO(string mealType)
        {
            IdCounter = IdCounter - 1;
            Id = IdCounter;
            ProgramId = 0;
            if (mealType.Equals("breakfast"))
            {
                MealTypeId = 1;
                Description = "breakfast";
            }
            else if (mealType.Equals("lunch"))
            {
                MealTypeId = 2;
                Description = "lunch";
            }
                
            else if (mealType.Equals("dinner"))
            {
                MealTypeId = 3;
                Description = "dinner";
            }
            StartTime = DateTime.Now;
            Description = "";
        }
        public MealDTO()
        {
        
        }

        [DisplayName("Meal id")]
        public int Id { get; set; }

        [DisplayName("Program id")]
        [Required]
        public int ProgramId { get; set; }

        [DisplayName("Meal Type")]
        [Required(ErrorMessage = "Choose meal type.")]
        public int MealTypeId { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
     //   [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter start time.")]
        public DateTime StartTime { get; set; }

        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }
    }
}