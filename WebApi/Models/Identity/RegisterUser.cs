using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Identity
{
    public class RegisterUser
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email!")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9_-]{3,25}$")]
        public string UserName { get; set; }


        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{1}[a-z]{1,15}$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{1}[a-z]{1,15}$")]
        public string Surname { get; set; }


    }
}
