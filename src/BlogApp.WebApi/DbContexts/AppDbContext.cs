﻿using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlogApp.WebApi.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();

            //modelBuilder.Entity<User>().HasData(new User
            //{
            //    FirstName = "Jon",
            //    LastName = "Doe",
            //    UserRole = UserRole.Admin,
            //    PasswordHash = "string",
            //    CreatedAt = DateTime.UtcNow,
            //    Email = "string",
            //    ImagePath = "string",
            //    IsEmailConfirmed = false,
            //    Salt = Guid.NewGuid().ToString(),
            //    UserName = "admin",
            //});
        }

        public virtual DbSet<User> Users { get; set; } = null!;

        public virtual DbSet<SaveMessage> SaveMessages { get; set; } = null!;

        public virtual DbSet<BlogPost> BlogPosts { get; set; } = null!;

    }
}