using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SidekickApp_WebAPI.SignalR;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Read the secret key from the environment variable
var secretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(secretKey))
{
    throw new InvalidOperationException("Invalid secret key");
}

var validAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>();
if (validAudiences == null || validAudiences.Count() == 0)
{
    throw new InvalidOperationException("Invalid valid audiences");
}

var validIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"];
if (string.IsNullOrWhiteSpace(validIssuer))
{
    throw new InvalidOperationException("Invalid valid issuer");
}

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudiences = validAudiences,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapHub<GPSLocationHub>("/GPSLocationHub");

app.Run();
