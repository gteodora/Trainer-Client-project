using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainerClientADOnet.Models.DTO
{
    public class RoleDTO
    {
        [DisplayName("Role id")]
        public string Id { get; set; }


        [DisplayName("Role name")]
        [Required]
        public string Name { get; set; }
    }
}