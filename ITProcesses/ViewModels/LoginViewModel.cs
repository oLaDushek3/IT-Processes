using System;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class LoginViewModel : BaseViewModel
{
    #region Fields

    private string _login = String.Empty;
    private string _password = String.Empty;
    private bool _checkBoxBool = false;
    private readonly UserService _userService = new UserService();
    private MainWindowViewModel _currentMainViewModel;

    #endregion

    #region Properties

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
    public bool CheckBoxBool
    {
        get => _checkBoxBool;
        set
        {
            if (value==_checkBoxBool) return;
            _checkBoxBool = value;
            OnPropertyChanged();
        }
    }
    public MainWindowViewModel CurrentMainViewModel
    {
        get => _currentMainViewModel;
        set
        {
            _currentMainViewModel = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public CommandHandler LoginCommand => new(LoginAsync);
    
    //public CommandHandler IsActive => new(SaveLoginIfoInJson);

    #endregion

    //Constructor
    public LoginViewModel(MainWindowViewModel currentMainViewModel)
    {
        CurrentMainViewModel = currentMainViewModel;
    }
    
    //Methods
    private async void LoginAsync()
    {
        try
        {
            await _userService.Login(Login, Password);
            
            if(CheckBoxBool==true)
                SaveLoginIfoInJson();

            CurrentMainViewModel.ChangeView(new MainViewModel());
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void SaveLoginIfoInJson()
    {
        var settings = Settings;
        settings!.UserName = Login;
        settings.Password = Password;
        SaveInfo.SaveSettings(settings);

    }
}