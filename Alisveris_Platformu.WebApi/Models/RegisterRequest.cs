﻿using System.ComponentModel.DataAnnotations;

namespace Alisveris_Platformu.WebApi.Models
{
    public class RegisterRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
