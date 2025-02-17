using ChatBot.Infrastructure.ExternalApis.AiConnector.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", 
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddHttpClient<AiService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAllOrigins");

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();