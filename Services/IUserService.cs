﻿using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IUserService
    {
        Task<HttpResponseMessage> RegisterUser(RegisterDTO registerDTO, HttpClient httpClient);

        Task<string> AuthenticateUser(UserDTO userDTO, HttpClient httpClient);
    }
}