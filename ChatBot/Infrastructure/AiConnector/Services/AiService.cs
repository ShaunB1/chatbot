using System.Text;
using System.Text.Json;
using DotNetEnv;

namespace ChatBot.Infrastructure.AiConnector.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    
    public AiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        Env.Load();
        
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("API key is missing.");
    }

    public async Task<string?> GetResponseAsync(string prompt)
    {
        const string systemContent = @"Please respond in raw Markdown format without triple backticks unless it contains code, 
        in which case, surround the code with triple backticks.";
        
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = systemContent },
                new { role = "user", content = prompt }
            },
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error: {responseBody}");
        }
        
        using var jsonDoc = JsonDocument.Parse(responseBody);
        
        return jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }
}