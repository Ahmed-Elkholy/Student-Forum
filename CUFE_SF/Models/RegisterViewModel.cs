using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CUFE_SF.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(15,ErrorMessage ="Name Should not exceed 15 characters")]
        [Remote("IsUserNameExist", "Validation", ErrorMessage = "User name already exists")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "ID is required.")]
        [Remote("IsIDExist", "Validation", ErrorMessage = "ID already exists")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Remote("IsEmailExist", "Validation", ErrorMessage = "Email already exists")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
    }
}