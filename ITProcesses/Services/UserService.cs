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

public class UserService : IUserService
{
    private readonly ItprocessesContext _context;

    public UserService(ItprocessesContext context)
    {
        _context = context;
    }

    public async Task<User> Login(string userName, string password)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
            throw new Exception("Неверный логин или пароль!");

        if (user.Password != password)
            throw new Exception("Неверный логин или пароль!");

        return user;
    }

    public async Task<User> Registration(User user)
    {
        var us = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

        if (us != null)
            throw new Exception("Логин занят");

        if (!ValidatePassword(user.Password))
            throw new Exception("Неверный формат пароля");
        user.Password = Hash.Md5.HashPassword(user.Password);

        user.Id = Guid.NewGuid();

        user.Password = Md5.HashPassword(user.Password);

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> Update(User user)
    {
        var us = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

        if (us != null)
            throw new Exception("Логин занят");
        
        if (!ValidatePassword(user.Password))
            throw new Exception("Неверный формат пароля");
        user.Password = Hash.Md5.HashPassword(user.Password);
        
        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.
            Include(u => u.Role).
            Include(u => u.Tasks).ToListAsync();
    }

    public async Task<User> GetUserById(Guid userId)
    {
        var user = await _context.Users.Include(u => u.Role).FirstAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("Не найден пользователь");

        return user;
    }

    public async Task<List<Role>> GetAllRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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