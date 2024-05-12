using System.Security.Cryptography;
using ITP.API.Models;
using ITProcesses.Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskModel = ITP.API.Models.Task;

namespace ITP.API.Controllers;

public class UserController(ItprocessesContext context) : Controller
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound();

        string passwordHas = Md5.HashPassword(password);

        if (user.Password != passwordHas)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllTasksById(Guid guid)
    {
        var userTasks = await context.UsersTasks.Where(uT => uT.UserId == guid)
            .Include(usersTask => usersTask.Task)
            .ToListAsync();

        if (userTasks == null)
            NotFound();
        
        var tasks = new List<TaskModel>();

        foreach (var item in userTasks)
        {
            tasks.Add(item.Task);
        }

        return Ok(tasks);
    }
}