﻿namespace BlogApp.WebApi.ViewModels.Users
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string LastName { get; set; } = String.Empty;

        public string FirstName { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public bool IsEmailConfirmed { get; set; } = false;

        public string ImagePath { get; set; } = String.Empty;
    }
}
