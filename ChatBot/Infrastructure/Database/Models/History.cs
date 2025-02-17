namespace ChatBot.Infrastructure.Database.Models;

public class History
{
    public int id { get; set; }
    public string message { get; set; }
    public string role { get; set; }
}