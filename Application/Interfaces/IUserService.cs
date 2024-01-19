﻿using Application.ViewModel;
using Domain.Models;
using Domain.ViewModels.User;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<State> Register(RegisterViewModel viewModel);
    public Task<bool> IsEmailRegistered(string Email);
    public Task<User> GetUserByActivateCode(string ActivateCode);

}