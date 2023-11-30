using System;
using System.Threading.Tasks;
using System.Windows;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.ViewModels;
using ITProcesses.Views;

namespace ITProcesses
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly UserService _userService = new(new());
        private static AppSettings? Settings => SaveInfo.AppSettings;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(await WriteUserFromJson());
            
            mainWindow.Show();
        }

        private async Task<User> WriteUserFromJson()
        {
            try
            {
                var user = await _userService.Login(Settings.UserName, Settings.Password);
                return user;
            }
            catch 
            {
                return null;
            }
        }
    }
}