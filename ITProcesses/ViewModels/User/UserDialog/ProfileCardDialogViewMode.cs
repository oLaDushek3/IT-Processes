using System;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Hash;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;

namespace ITProcesses.ViewModels;

public class ProfileCardDialogViewMode : BaseViewModel
{
    #region Fields

    private User _profileUser;
    private DialogProvider _currentDialogProvider;
    private MainWindowViewModel _currentMainWindowViewModel;
    private bool _lightTheme;
    private bool _darkTheme;

    #endregion

    #region Properties

    public DialogProvider ConfirmDialogProvider { get; } = new();

    public User ProfileUser
    {
        get => _profileUser;
        set
        {
            _profileUser = value;
            OnPropertyChanged();
        }
    }

    public bool LightTheme
    {
        get => _lightTheme;
        set
        {
            _lightTheme = value;
            OnPropertyChanged();
        }
    }

    public bool DarkTheme
    {
        get => _darkTheme;
        set
        {
            _darkTheme = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler LogOutCommand => new(_ => LogOutCommandExecute());

    public CommandHandler ChangeThemeCommand => new(_ => ChangeThemeCommandExecute());

    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    //Constructor
    public ProfileCardDialogViewMode(User profileUser, DialogProvider currentDialogProvider,
        MainWindowViewModel currentMainWindowViewModel)
    {
        ProfileUser = profileUser;
        _currentDialogProvider = currentDialogProvider;
        _currentMainWindowViewModel = currentMainWindowViewModel;

        if (Settings.CurrentTheme == "UILightColors")
            LightTheme = true;
        else
            DarkTheme = true;
    }

    //Methods
    private async void LogOutCommandExecute()
    {
        if ((bool)await ConfirmDialogProvider.ShowDialog(
                new ConfirmDialogViewModel(ConfirmDialogProvider)))
        {
            _currentDialogProvider.CloseDialog(null);
            _currentMainWindowViewModel.ChangeView(new LoginViewModel(_currentMainWindowViewModel));
            SaveInfo.CreateAppSettingsDefault();

            var app = (App)Application.Current;
            app.ChangeTheme(new Uri("Resources/" + Settings.CurrentTheme + ".xaml", UriKind.RelativeOrAbsolute));
        }
    }

    private void ChangeThemeCommandExecute()
    {
        string selectedTheme;

        selectedTheme = _darkTheme
            ? "UIDarkColors"
            : "UILightColors";

        var app = (App)Application.Current;
        app.ChangeTheme(new Uri("Resources/" + selectedTheme + ".xaml", UriKind.RelativeOrAbsolute));
        
        AppSettings settings = SaveInfo.ReadAppSettings();
        settings.CurrentTheme = selectedTheme;
        SaveInfo.SaveSettings(settings);
    }
}