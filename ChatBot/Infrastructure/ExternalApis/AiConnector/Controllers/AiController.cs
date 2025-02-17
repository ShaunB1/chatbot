using System.Text.Json;
using ChatBot.Infrastructure.ExternalApis.AiConnector.Models;
using ChatBot.Infrastructure.ExternalApis.AiConnector.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Infrastructure.ExternalApis.AiConnector.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly AiService _aiService;
    private readonly HttpClient _httpClient;
    private readonly string _dbUrl;

    public AiController(HttpClient httpClient, AiService aiService)
    {
        _aiService = aiService;
        _httpClient = httpClient;
        
        var host = "localhost:5292";
        
        _dbUrl = $"http://{host}/api/db/history";
    }

    [HttpPost("chat")]
    public async Task<IActionResult> GenerateResponse([FromBody] ChatRequest request)
    {
        try
        {
            var dbResponse = await _httpClient.GetAsync(_dbUrl);

            if (!dbResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)dbResponse.StatusCode, "Error fetching chat history.");
            }
            
            var jsonString = await dbResponse.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<Message>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { error = "Prompt cannot be empty." });
            }
            
            data ??= new List<Message>();
            data.Add(new Message { message = request.Prompt, role = "user" });
        
            var response = await _aiService.GetResponseAsync(data);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class Message
{
    public string message { get; set; }
    public string role { get; set; }
}