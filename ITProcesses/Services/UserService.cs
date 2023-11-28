using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ITProcesses.Hash;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Services;

public class UserService : BaseViewModel, IUserService
{
    public async Task<User> Login(string userName, string password)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
            throw new Exception("Неверный логин или пароль!");
        
        if (user.Password != password)
            throw new Exception("Неверный логин или пароль!");
        
        return user;
    }

    public async Task<User> Registration(User user)
    {
        var us = await Context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

        if (us != null)
            throw new Exception("Данный пользователь уже существует");

        if (!ValidatePassword(user.Password))
            throw new Exception("Неверный формат пароля");

        //Если будешь писать лоигку добавления, то не забывай, что guid не identity и тебе надо вручную писать!!!
        user.Id = Guid.NewGuid();

        user.Password = Md5.HashPassword(user.Password);

        await Context.Users.AddAsync(user);

        await Context.SaveChangesAsync();

        return user;
    }

    public async Task<User> Update(User user)
    {
        Context.Users.Update(user);

        await Context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await Context.Users.
            Include(u => u.Role).ToListAsync();
    }

    public async Task<List<Role>> GetAllRoles()
    {
        return await Context.Roles.ToListAsync();
    }
    
    private bool ValidatePassword(string password)
    {
        return password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Length >= 8;
    }
}