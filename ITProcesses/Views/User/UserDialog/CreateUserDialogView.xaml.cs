using System.Windows;
using System.Windows.Controls;

namespace ITProcesses.Views;

public partial class CreateUserDialogView : UserControl
{
    public CreateUserDialogView()
    {
        InitializeComponent();
    }
    
    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
    }
    
    private void ConfirmPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        ((dynamic)DataContext).ConfirmPassword = ((PasswordBox)sender).Password;
    }
    
}