using System.Text;
using System.Text.Json;
using DotNetEnv;

namespace ChatBot.Infrastructure.ExternalApis.AiConnector.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly string _aiModel;
    private const string OpenAiEndpoint = "https://api.openai.com/v1/chat/completions";
    
    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        
        Env.Load();
        
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("API key is missing.");
        _aiModel = configuration["ChatBotSettings:Model"] ?? throw new Exception("AI model is missing.");
        
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string?> GetResponseAsync(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("Prompt is missing.");
        }
        
        try
        {
            const string systemContent = @"Please respond in raw Markdown format without triple backticks unless 
            it contains code, in which case, surround the code with triple backticks. It uses syntax highlighting too 
            so provide the language. Ex. 
            ```js
            console.log(""Hello, World!"");";
        
            var requestBody = new
            {
                model = _aiModel,
                messages = new[]
                {
                    new { role = "system", content = systemContent },
                    new { role = "user", content = prompt }
                },
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestBody), 
                Encoding.UTF8,
                "application/json"
            );
            
            var response = await _httpClient.PostAsync(OpenAiEndpoint, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {responseBody}");
            }
        
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
}