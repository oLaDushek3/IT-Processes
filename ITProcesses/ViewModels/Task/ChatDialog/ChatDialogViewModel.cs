using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using ITProcesses.API;
using ITProcesses.Command;
using ITProcesses.Models;

namespace ITProcesses.ViewModels.ChatDialog;

public class ChatDialogViewModel : BaseViewModel
{
    private readonly ItprocessesContext _context = new();
    private List<ChatMessage> _messagesList;
    private Guid _guid;
    private ChatMessage _newChatMessage = new ChatMessage();
    private User _user = new User()
        ;

    private ObservableCollection<ChatMessage> _displayedMessages;

    private string _message = string.Empty;

    private ApiUpdate _apiUpdate;

    public string Message
    {
        get => _message;
        set
        {
            if (value == _message) return;
            _message = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ChatMessage> DisplayedChatMessagesList
    {
        get => _displayedMessages;
        set
        {
            _displayedMessages = value;
            OnPropertyChanged();
        }
    }

    public CommandHandler SubmitMessageCommand => new(_ => CreateMessageCommandExecute());

    public ChatDialogViewModel(Guid guid, User user)
    {
        _apiUpdate = new ApiUpdate(_context);
        _guid = guid;
        _displayedMessages = new ObservableCollection<ChatMessage>();
        _user = user;

        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += TimerOnTick;
        timer.Start();
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        GetData();
    }

    private async void GetData()
    {
        _messagesList = new List<ChatMessage>();
        _messagesList = await _apiUpdate.GetAllMessagesByTaskId(_guid, 0);
        DisplayedChatMessagesList = new ObservableCollection<ChatMessage>(_messagesList);
        OnPropertyChanged();
    }

    private async void CreateMessageCommandExecute()
    {
        _newChatMessage.Message = Message;
        _newChatMessage.TaskId = _guid;
        _newChatMessage.UsersId = _user.Id;
        try
        {
            await _apiUpdate.AddNewMessage(_newChatMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
}