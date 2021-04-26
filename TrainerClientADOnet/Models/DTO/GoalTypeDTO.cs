using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainerClientADOnet.Models.DTO
{
    public class GoalTypeDTO 
    {
        [DisplayName("Goal type id")]
        public int Id { get; set; }


        [DisplayName("Goal type")]
        [Required]
        public string Name { get; set; }
    }
}