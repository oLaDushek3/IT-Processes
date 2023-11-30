using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;

namespace ITProcesses.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public static AppSettings? Settings =>
            SaveInfo.AppSettings;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}