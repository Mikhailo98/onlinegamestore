using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Infrastucture
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        Inherited = false, AllowMultiple = false)]
    public class CustomIDsValidationAttribute : ValidationAttribute
    {
        

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var result = value as List<int>;
            string errorMessage = string.Empty;
            bool IsValid = true;


            if (result.Any(p => p <= 0))
            {
                errorMessage += "ID should not be zero or less.";
                IsValid = false;
            }

            if (result.Distinct().Count() != result.Count)
            {
                errorMessage += " ID values should be distinct.";
                IsValid = false;
            }

            if (IsValid == false)
            {
               this.ErrorMessage = errorMessage;
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;

        }
    }
}
