﻿using Domain.Models;
using Domain.ViewModels.User;
namespace Application.Interfaces;

public interface IUserService
{
    public Task<State> Register(RegisterViewModel viewModel);
    public Task<bool> IsEmailRegistered(string Email);
    public Task<User> GetUserByActivateCode(string ActivateCode);
    public Task GiveUserActiveRole(User user);
    public Task<UserViewModel> GetUserEmail(string Email);
    public Task<LoginResult> LoginUser(LoginViewModel user);
    public Task<UserViewModel> GetUserById(int id);
    public Task<ICollection<UserViewModel>> GetUsers();
    public Task DeleteUser(int id);
    public Task EditUser(UserViewModel viewModel);
    public Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser);
    public Task ForgotPassword(ForgotPasswordViewModel model);
    public Task<bool> IsUserExistById(int Id);
    public Task EditUserInfo(EditUserViewModel model,int id);

    public Task ActiveMobile(int UserId,string Code);
}