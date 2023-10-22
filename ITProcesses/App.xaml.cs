using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Services;

namespace ITProcesses
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly UserService _userService = new();
        private static AppSettings? Settings => SaveInfo.AppSettings;
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                SaveInfo.CreateAppSettingsDefault();
                WriteUserFromJson();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private async void WriteUserFromJson()
        {
            try
            {
                await _userService.Login(Settings.UserName, Settings.Password);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}