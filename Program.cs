using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using youtubeApi.Model;  // Make sure you have the correct namespaces
using youtubeApi.Services; // Ensure these services are defined properly
using youtubeApi.Servics; // Check if this namespace is needed

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();

// Configure email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register services
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<EmailService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});


// Register CodeStore as a singleton
builder.Services.AddSingleton<CodeStore>();

var app = builder.Build();

// Middleware configuration
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

// Map controllers
app.MapControllers();


app.Run();
