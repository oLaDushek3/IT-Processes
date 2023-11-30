using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface IUserService
{
     Task<User> Login(string login, string password);

     Task<User> Registration(User user);

     Task<User> Update(User user);

     Task<List<User>> GetAllUsers();
     
     Task<User> GetUserById(Guid userId);
     
     Task<List<Role>> GetAllRoles();

     Task DeleteUser(User user);
}