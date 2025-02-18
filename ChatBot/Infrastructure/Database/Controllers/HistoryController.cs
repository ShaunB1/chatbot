using ChatBot.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Infrastructure.Database.Controllers;

[Route("api/db")]
[ApiController]
public class HistoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public HistoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<History>>> GetHistoryAsync()
    {
        try
        {
            var history = await _context.History.ToListAsync();

            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("history")]
    public async Task<IActionResult> AddMessageAsync([FromBody] History request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.message))
            {
                return BadRequest(new { message = "Message cannot be empty." });
            }
            
            await _context.History.AddAsync(request);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Successfully added message to chat history." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("history")]
    public async Task<IActionResult> DeleteHistoryAsync()
    {
        try
        {
            var allHistory = await _context.History.ToListAsync();

            if (allHistory.Count == 0)
            {
                return NotFound(new { message = "Chat history is empty." });
            }
            
            _context.History.RemoveRange(allHistory);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Chat history has been deleted." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}