using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ITProcesses.Models;
using Type = System.Type;

namespace ITProcesses.HelpersAndConverters;

public class UserRoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var userRole = (value as User).Role;
        if (userRole.Id == 1)
            return Visibility.Visible;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}