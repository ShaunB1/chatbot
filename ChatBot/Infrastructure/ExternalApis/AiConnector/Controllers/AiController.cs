using ChatBot.Infrastructure.ExternalApis.AiConnector.Models;
using ChatBot.Infrastructure.ExternalApis.AiConnector.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Infrastructure.ExternalApis.AiConnector.Controllers;

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
        try
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { error = "Prompt cannot be empty." });
            }
        
            var response = await _aiService.GetResponseAsync(request.Prompt);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}