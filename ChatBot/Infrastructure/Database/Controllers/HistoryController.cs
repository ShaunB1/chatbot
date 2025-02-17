using ChatBot.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Infrastructure.Database.Controllers;

[Route("api/db/history")]
[ApiController]
public class HistoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public HistoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<History>>> GetHistoryAsync()
    {
        try
        {
            var history = await _context.History.ToListAsync();

            if (history.Count == 0)
            {
                return NotFound();
            }

            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}