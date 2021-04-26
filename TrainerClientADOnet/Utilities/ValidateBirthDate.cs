using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc.Async;

namespace TrainerClientADOnet.Utilities
{
    public class ValidateBirthnDate : ValidationAttribute
    {
        private DateTime _maxDate;
        public ValidateBirthnDate(int years)
        {
            _maxDate = DateTime.Now.AddYears(years);
        }
      //  public string GetErrorMessage() =>
      //  $"Classic movies must have a release year no later than {_maxDate}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            // 
               if ((DateTime)value <= _maxDate)
               {
                   return ValidationResult.Success;
               }
               else
               {
                   return new ValidationResult("Person must be older than 10.");
               }
        }
    }
}