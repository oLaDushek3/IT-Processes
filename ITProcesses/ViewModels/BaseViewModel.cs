using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;

namespace ITProcesses.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    protected readonly ItprocessesContext Context = new ();
    
    public static AppSettings? Settings =>
            SaveInfo.AppSettings;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}