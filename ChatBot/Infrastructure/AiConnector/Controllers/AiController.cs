using ChatBot.Infrastructure.AiConnector.Models;
using ChatBot.Infrastructure.AiConnector.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Infrastructure.AiConnector.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly AiService _aiService;

    public AiController(AiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> GenerateResponse([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest("Prompt cannot be empty.");
        }
        
        var response = await _aiService.GetResponseAsync(request.Prompt);
        return Ok(response);
    }
}