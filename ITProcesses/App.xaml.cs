using System;
using System.Threading.Tasks;
using System.Windows;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.ViewModels;

namespace ITProcesses
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly UserService _userService = new();
        private static AppSettings? Settings => SaveInfo.AppSettings;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            
            try
            {
                SaveInfo.CreateAppSettingsDefault();
                mainWindow.DataContext = new MainWindowViewModel(WriteUserFromJson());
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка");
            }
            
            mainWindow.Show();
        }

        private bool WriteUserFromJson()
        {
            try
            {
                _userService.Login(Settings.UserName, Settings.Password);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }
    }
}