using System;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _login = String.Empty;
    private string _password = String.Empty;
    private readonly UserService _userService = new UserService();
    public LoginViewModel()
    {
        
    }

    public string Login
    {
        get => _login;
        set
        {
            if (value == _login) return;
            _login = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value==_password) return;
            _password = value;
            OnPropertyChanged();
        }
    }

    public CommandHandler LoginCommand => new(LoginAsync);

    //public CommandHandler IsActive => new(SaveLoginIfoInJson);

    private async void LoginAsync()
    {
        try
        {
            await _userService.Login(Login, Password);
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private async void SaveLoginIfoInJson()
    {
        
    }
}