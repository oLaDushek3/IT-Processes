using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ITProcesses.ViewModels;

namespace ITProcesses.Dialog;

public class DialogProvider : INotifyPropertyChanged
{
    #region Fields

    private BaseViewModel? _currentDialogView;
    private bool _dialogActive;
    private object? _dialogResult;

    #endregion

    #region Properties

    public BaseViewModel? DialogView 
    {
        get => _currentDialogView;
        set
        {
            _currentDialogView = value;
            OnPropertyChanged();
        }
    }
    public bool DialogActive
    {
        get => _dialogActive;
        set
        {
            _dialogActive = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    private delegate void CloseDialogDelegate();
    private event CloseDialogDelegate CloseDialogEvent;
    
    public Task<object> ShowDialog(BaseViewModel currentDialogView)
    {
        DialogView = currentDialogView;

        DialogActive = true;

        var completion = new TaskCompletionSource<object>();

        CloseDialogEvent += () => completion.TrySetResult(_dialogResult);
        return completion.Task;
    }
    
    public void CloseDialog(object? dialogResult)
    {
        _dialogResult = dialogResult;
        DialogView = null;
        DialogActive = false;

        CloseDialogEvent?.Invoke();
    }

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}