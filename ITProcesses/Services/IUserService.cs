﻿using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface IUserService
{
     Task<User> Login(string login, string password);

     Task<User> Registration(User user);

     Task<User> Update(User user);

     Task<User> GetUserByLoginAndPassword(string login, string password);

}