using System;
using System.Collections.Generic;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using Microsoft.Win32;

namespace ITProcesses.ViewModels;

public class EditUserDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;

    private readonly ItprocessesContext _context = new();
    private readonly IUserService _userService;
    private readonly DialogProvider _errorDialogProvider;

    private List<Role> _roleList;
    private User _editableUser;
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

    public User EditableUser
    {
        get => _editableUser;
        set
        {
            _editableUser = value;
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

    public CommandHandler SaveCommand => new(_ => SaveCommandExecute(),
        _ => EditableUser != null &&
             !string.IsNullOrEmpty(EditableUser.LastName) &&
             !string.IsNullOrEmpty(EditableUser.FirstName) &&
             !string.IsNullOrEmpty(EditableUser.MiddleName) &&
             !string.IsNullOrEmpty(EditableUser.Username) &&
             EditableUser.Role != null);

    //Constructor
    public EditUserDialogViewModel(DialogProvider errorDialogProvider, User selectedUser, DialogProvider currentDialogProvider)
    {
        _errorDialogProvider = errorDialogProvider;
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData(selectedUser.Id);
    }

    //Methods
    private async void GetData(Guid userId)
    {
        RoleList = await _userService.GetAllRoles();
        EditableUser = await _userService.GetUserById(userId);
    }

    private void SelectImageCommandExecute()
    {
        var fileDialog = new OpenFileDialog
        {
            Filter ="Добавление изображения | *.png; *.jpg"
        };
        
        if ((bool)fileDialog.ShowDialog()!)
        {
            EditableUser.Image = fileDialog.FileName;
            OnPropertyChanged("EditableUser");
        }
    }
    
    private async void SaveCommandExecute()
    {
        try
        {
            if (!string.IsNullOrEmpty(Password))
            {
                if (Password != ConfirmPassword)
                {
                    _errorDialogProvider.ShowDialog(new ErrorDialogViewModel(_errorDialogProvider, 
                        "Пароли не совпадают"));
                    return;
                }
                EditableUser.Password = Password;
            }

            await _userService.Update(EditableUser);
            _currentDialogProvider.CloseDialog(EditableUser);
        }
        catch (Exception e)
        {
            _errorDialogProvider.ShowDialog(new ErrorDialogViewModel(_errorDialogProvider, e.ToString()));
        }
    }
}