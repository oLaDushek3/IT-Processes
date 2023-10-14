using System;
using System.Threading.Tasks;
using System.Windows;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Services;

public class UserService:BaseViewModel, IUserService
{
    public async Task<User> Login(string userName, string password)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
            throw new Exception("Пользователь не найден");

        if (user.Password != password)
            throw new Exception("Нверный пароль");
        
        return user;
    }
}