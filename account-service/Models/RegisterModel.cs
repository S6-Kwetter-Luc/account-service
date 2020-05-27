﻿using System.ComponentModel.DataAnnotations;

namespace account_service.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Bio { get; set; }
    }
}