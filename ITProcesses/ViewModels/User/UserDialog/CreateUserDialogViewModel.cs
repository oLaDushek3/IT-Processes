using System;
using System.Collections.Generic;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using Microsoft.Win32;

namespace ITProcesses.ViewModels;

public class CreateUserDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;

    private readonly ItprocessesContext _context = new();
    private readonly IUserService _userService;
    private readonly DialogProvider _errorDialogProvider;

    private List<Role> _roleList;
    private User _createdUser = new();
    private string _password;
    private string _confirmPassword;

    #endregion

    #region Properties

    public List<Role> RoleList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            OnPropertyChanged();
        }
    }

    public User CreatedUser
    {
        get => _createdUser;
        set
        {
            _createdUser = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler SelectImageCommand => new(_ => SelectImageCommandExecute());
    
    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler CreateCommand => new(_ => CreateCommandExecute(),
        _ => CreatedUser != null &&
             !string.IsNullOrEmpty(CreatedUser.LastName) &&
             !string.IsNullOrEmpty(CreatedUser.FirstName) &&
             !string.IsNullOrEmpty(CreatedUser.MiddleName) &&
             !string.IsNullOrEmpty(CreatedUser.Username) &&
             !string.IsNullOrEmpty(Password) &&
             !string.IsNullOrEmpty(ConfirmPassword) &&
             CreatedUser.Role != null);

    //Constructor
    public CreateUserDialogViewModel(DialogProvider errorDialogProvider, DialogProvider currentDialogProvider)
    {
        _errorDialogProvider = errorDialogProvider;
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        RoleList = await _userService.GetAllRoles();
    }

    private void SelectImageCommandExecute()
    {
        var fileDialog = new OpenFileDialog
        {
            Filter = "Добавление изображения | *.png; *.jpg"
        };

        if ((bool)fileDialog.ShowDialog()!)
        {
            CreatedUser.Image = fileDialog.FileName;
            OnPropertyChanged("CreatedUser");
        }
    }

    private async void CreateCommandExecute()
    {
        try
        {
            if (Password != ConfirmPassword)
            {
                _errorDialogProvider.ShowDialog(new ErrorDialogViewModel(_errorDialogProvider,
                    "Пароли не совпадают"));
                return;
            }
            CreatedUser.Password = Password;
            
            await _userService.Registration(CreatedUser);
            _currentDialogProvider.CloseDialog(CreatedUser);
        }
        catch (Exception e)
        {
            _errorDialogProvider.ShowDialog(new ErrorDialogViewModel(_errorDialogProvider, e.ToString()));
        }
    }
}