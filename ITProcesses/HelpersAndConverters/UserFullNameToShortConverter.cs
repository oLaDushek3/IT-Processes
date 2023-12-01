using System.Globalization;
using System.Windows.Data;
using ITProcesses.Models;

namespace ITProcesses.HelpersAndConverters;

public class UserFullNameToShortConverter : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        User? user = (User?)value;
        
        if (user == null) return null;
        
        return user.LastName + " " + user.FirstName[0] + ". " + user.MiddleName[0] + ".";
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}