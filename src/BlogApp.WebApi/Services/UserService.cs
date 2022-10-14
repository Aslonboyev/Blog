﻿using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.Enums;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Models;
using BlogApp.WebApi.Security;
using BlogApp.WebApi.Utills;
using BlogApp.WebApi.ViewModels.Users;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepositroy;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserService(IUserRepository userRepositroy, IFileService fileService, IWebHostEnvironment hostingEnvironment)
        {
            _userRepositroy = userRepositroy;
            _fileService = fileService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
        {
            var result = await _userRepositroy.GetAsync(expression);

            if (result is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            if (HttpContextHelper.UserId != result.Id && HttpContextHelper.UserRole == UserRole.User.ToString())
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");
            
            string webRootPath = _hostingEnvironment.WebRootPath;
            
            var fullPath = webRootPath + "\\" + result.ImagePath;

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            result.ItemState = ItemState.Inactive;

            await _userRepositroy.UpdateAsync(result);

            await _userRepositroy.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync(PaginationParams? pagination = null, Expression<Func<User, bool>>? expression = null)
        {
            return (from user in _userRepositroy.GetAllAsync(expression)
                    orderby user.CreatedAt descending
                    select (UserViewModel)user).ToPaged(pagination);
        }

        public async Task<UserViewModel> GetAsync(Expression<Func<User, bool>> expression)
        {
            var user = await _userRepositroy.GetAsync(expression);

            if (user == null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            return (UserViewModel)user;
        }

        public async Task<bool> UpdateAsync(long id, UserPatchViewModel model)
        {
            var user = await _userRepositroy.GetAsync(o => o.Id == id && o.ItemState == ItemState.Active);

            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

            if (HttpContextHelper.UserId != id)
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");

            if (model.Image is not null)
                user.ImagePath = await _fileService.SaveImageAsync(model.Image);
            
            if(model.FirstName is not null)
                user.FirstName = model.FirstName;

            if(model.LastName is not null)
                user.LastName = model.LastName;

            if (model.Email is not null)
            { 
                var email = await _userRepositroy.GetAsync(o => o.Email == model.Email); 
                
                if (user.Id != email.Id)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Email have already taken");
            }

            if (model.UserName is not null)
            {
                var username = await _userRepositroy.GetAsync(o => o.UserName == model.UserName);

                if (user.Id != username.Id)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Username have already taken");

                user.UserName = model.UserName;
            }

            await _userRepositroy.UpdateAsync(user);

            await _userRepositroy.SaveAsync();

            return true;
        }

        //public async Task<bool> UpdateAsync(long id, UserCreateViewModel viewModel)
        //{
        //    var user = await _userRepositroy.GetAsync(o => o.Id == id && o.ItemState == ItemState.Active);

        //    if (user is null)
        //        throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found");

        //    if (HttpContextHelper.UserId != id)
        //        throw new StatusCodeException(HttpStatusCode.BadRequest, message: "must enter correct id");

        //    user.FirstName = viewModel.FirstName;
        //    user.LastName = viewModel.LastName;
            
        //    var username = await _userRepositroy.GetAsync(o => o.UserName == viewModel.UserName);
        //    if (user.Id != username.Id)
        //        throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Username have already taken");
            
        //    user.UserName = viewModel.UserName;
            
        //    var email = await _userRepositroy.GetAsync(o => o.Email == viewModel.Email);
        //    if (user.Id != email.Id)
        //        throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Email have already taken");
            
        //    user.Email = viewModel.Email;

        //    user.PasswordHash = PasswordHasher.ChangePassword(viewModel.Password, user.Salt);

        //    await _userRepositroy.UpdateAsync(user);

        //    await _userRepositroy.SaveAsync();

        //    return true;
        //}
    }
}
