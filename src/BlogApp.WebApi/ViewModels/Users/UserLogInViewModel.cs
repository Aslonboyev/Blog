﻿using BlogApp.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class UserLogInViewModel
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StrongPassword]    
        public string Password { get; set; } = string.Empty;
    }
}