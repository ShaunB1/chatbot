using System.Text;
using System.Text.Json;
using ChatBot.Infrastructure.ExternalApis.AiConnector.Controllers;
using DotNetEnv;

namespace ChatBot.Infrastructure.ExternalApis.AiConnector.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private const string OpenAiEndpoint = "https://api.openai.com/v1/chat/completions";
    private const string ClaudeEndpoint = "https://api.anthropic.com/v1/messages";
    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetResponseAsync(string model, List<Message> chatMessages)
    {
        try
        {
            object requestBody;
            string apiKey;

            var request = new HttpRequestMessage(HttpMethod.Post, model.Contains("gpt") ? OpenAiEndpoint : ClaudeEndpoint);
            
            if (model.Contains("gpt"))
            {
                requestBody = BuildOpenAiRequest(model, chatMessages);
                apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("Missing OpenAI key.");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }
            else if (model.Contains("claude"))
            {
                requestBody = BuildClaudeRequest(model, chatMessages);
                apiKey = Environment.GetEnvironmentVariable("CLAUDE_API_KEY") ?? throw new Exception("Missing Claude key.");
                request.Headers.Add("x-api-key", apiKey);
                request.Headers.Add("anthropic-version", "2023-06-01");
            }
            else
            {
                throw new Exception("Invalid model.");
            }
            
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody), 
                Encoding.UTF8,
                "application/json"
            );
            
            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {responseBody}");
            }
            
            var responseString = model.Contains("gpt") 
                ? ParseOpenAiResponse(responseBody) 
                : ParseClaudeResponse(responseBody);
            return responseString;
            
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
    }

    private string? ParseOpenAiResponse(string responseBody)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(responseBody);
            
            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
    }
    
    private string? ParseClaudeResponse(string responseBody)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(responseBody);
            var contentArray = jsonDoc.RootElement.GetProperty("content");

            if (contentArray.ValueKind == JsonValueKind.Array && contentArray.GetArrayLength() > 0)
            {
                var firstContent = contentArray[0];

                if (firstContent.TryGetProperty("text", out var message))
                {
                    return message.GetString();
                }
            }

            throw new Exception($"Error: {responseBody}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
    }
    
    private object BuildOpenAiRequest(string model, List<Message> chatMessages)
    {
        try
        {
            const string systemContent = @"Please respond in raw Markdown format without triple backticks unless 
            it contains code, in which case, surround the code with triple backticks. It uses syntax highlighting too 
            so provide the language. Ex. 
            ```js
            console.log(""Hello, World!"");";
        
            return new
            {
                model,
                messages = new[]
                    {
                        new { role = "system", content = systemContent },
                    }
                    .Concat<object>(chatMessages.Select(msg => new { msg.role, content = msg.message }))
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
    }

    private object BuildClaudeRequest(string model, List<Message> chatMessages)
    {
        try
        {
            const string systemContent = @"Please respond in raw Markdown format without triple backticks unless 
            it contains code, in which case, surround the code with triple backticks. It uses syntax highlighting too 
            so provide the language. Ex. 
            ```js
            console.log(""Hello, World!"");";

            var systemMessage = new Message
            {
                role = "user",
                message = systemContent,
            };
            
            var updatedChatMessages = new List<Message> { systemMessage };

            foreach (var msg in chatMessages)
            {
                updatedChatMessages.Add(new Message
                {
                    role = msg.role == "system" ? "user" : msg.role,
                    message = msg.message,
                });
            }
            
            return new
            {
                model,
                max_tokens = 300,
                messages = updatedChatMessages.Select(msg => new { msg.role, content = msg.message }).ToList()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
    }
}