namespace ChatBot.Infrastructure.ExternalApis.AiConnector.Models;

public class ChatRequest
{
    public required string Message { get; set; }
    public required string Role { get; set; }
    public required string Model { get; set; }
}