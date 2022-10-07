﻿using BlogApp.WebApi.Attributes;
using BlogApp.WebApi.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostCreateViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = String.Empty;

        [Required]
        [MinLength(50)]
        public string Description { get; set; } = String.Empty;

        [Required]
        public string Type { get; set; } = String.Empty;

        public string Subtitle { get; set; } = String.Empty;

        [DataType(DataType.Upload)]
        [MaxFileSize(3)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; } = null!;
        [Required]
        public ulong UserId { get; set; }

        public static implicit operator BlogPost(BlogPostCreateViewModel model)
        {
            return new BlogPost
            {
                Title = model.Title,
                SubTitle = model.Subtitle,
                Type = model.Type,
                Description = model.Description,
                UserId = model.UserId,
            };
        }
    }
}
