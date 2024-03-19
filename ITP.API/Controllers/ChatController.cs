using ITP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace ITP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : Controller
{
    private readonly ItprocessesContext _context;

    public ChatController(ItprocessesContext context)
    {
        _context = context;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllMessageByTaskId(Guid id, int count)
    {
        var messages = await _context.ChatMessages.Include(c => c.Users)
            .OrderByDescending(c => c.CreatedDate)
            .Where(cm => cm.TaskId == id)
            .Skip(count).Take(50).ToListAsync();

        if (messages == null)
            return NotFound();

        return Ok(messages);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddNewMessage(ChatMessage chatMessage)
    {
        await _context.AddAsync(chatMessage);

        await _context.SaveChangesAsync();

        return Ok(chatMessage);
    }
}