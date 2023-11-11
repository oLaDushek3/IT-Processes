using System;
using System.Windows;
using System.Windows.Input;
using ITProcesses.Command;
using ITProcesses.Hash;
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
    private MainWindowViewModel _currentMainWindowViewModel;

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
    public MainWindowViewModel CurrentMainWindowViewModel
    {
        get => _currentMainWindowViewModel;
        set
        {
            _currentMainWindowViewModel = value;
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
        CurrentMainWindowViewModel = currentMainViewModel;
    }
    
    //Methods
    private async void LoginAsync()
    {
        try
        {
          var user =  await _userService.Login(Login, Md5.HashPassword(Password));
            
            if(CheckBoxBool==true)
                SaveLoginIfoInJson();

            CurrentMainWindowViewModel.ChangeView(new MainViewModel(CurrentMainWindowViewModel,user));
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
        settings.Password = Md5.HashPassword(Password);
        SaveInfo.SaveSettings(settings);

    }
}