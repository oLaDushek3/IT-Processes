using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using ITProcesses.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace ITProcesses.API;

public class ApiUpdate : ApiBase
{

    

    public async Task<List<ChatMessage>> GetAllMessagesByTaskId(Guid id, int count)
    {
        try
        {
            var client = HttpClient;

            var res = await client.GetStringAsync($"api/Chat/GetAllMessageByTaskId?id={id}&count={count}");

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var chatMessages = JsonSerializer.Deserialize<List<ChatMessage>>(res, options) ??
                               throw new InvalidOperationException();

            // foreach (var item in chatMessages)
            // {
            //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == item.UsersId);
            //     item.Users = user;
            // }

            return chatMessages;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            // SaveFileDialog saveFileDialog = new SaveFileDialog
            // {
            //     Filter = "Excel files(*.xlsx)|*.xlsx"
            // };
            //
            // if (saveFileDialog.ShowDialog() == false)
            //     throw;
            // получаем выбранный файл
            // string filename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //
            // await using StreamWriter writer = new StreamWriter(filename, false);
            // await writer.WriteLineAsync(e.Message);
            throw;
        }
    }

    public async Task<bool> AddNewMessage(ChatMessage userMessage)
    {
        try
        {
            var client = HttpClient;

            var res = await client.PostAsJsonAsync($"api/Chat/AddNewMessage", userMessage);

            return res.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // SaveFileDialog saveFileDialog = new SaveFileDialog
            // {
            //     Filter = "Excel files(*.xlsx)|*.xlsx"
            // };
            //
            // if (saveFileDialog.ShowDialog() == false)
            //     throw;
            // // получаем выбранный файл
            // string filename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //
            // await using StreamWriter writer = new StreamWriter(filename, false);
            // await writer.WriteLineAsync(e.Message);
            throw;
        }
    }
}