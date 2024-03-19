using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using ITProcesses.Models;

namespace ITProcesses.API;

public class ApiUpdate : ApiBase
{
    private ItprocessesContext _context;

    public ApiUpdate(ItprocessesContext context)
    {
        _context = context;
    }

    public async Task<List<ChatMessage>> GetAllMessagesByTaskId(Guid id, int count)
    {
        var client = HttpClient;

        var res = await client.GetStringAsync($"api/Chat/GetAllMessageByTaskId?id={id}&count={count}");

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<ChatMessage>>(res, options) ?? throw new InvalidOperationException();
    }

    public async Task<bool> AddNewMessage(ChatMessage userMessage)
    {
        var client = HttpClient;

        var res = await client.PostAsJsonAsync($"api/Chat/AddNewMessage", userMessage);
        
        return res.IsSuccessStatusCode;
    }
}