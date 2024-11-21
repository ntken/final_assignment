using CarStore.Data;
using CarStore.EndPoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("CarStore");
builder.Services.AddSqlite<CarStoreContext>(connString);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Đảm bảo đọc JwtKey và JwtIssuer từ appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
Console.WriteLine($"JWT Issuer: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"JWT Key: {builder.Configuration["Jwt:Key"]}");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
{
    throw new Exception("Jwt:Key and Jwt:Issuer must be configured in appsettings.json");
}
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.ClaimsIssuer = builder.Configuration["Jwt:Issuer"];
        options.Audience = builder.Configuration["Jwt:Issuer"];
        options.Authority = builder.Configuration["Jwt:Issuer"];
        options.UseSecurityTokenValidators = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = false,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // "http://localhost:5237"
            ValidAudience = builder.Configuration["Jwt:Issuer"], // "http://localhost:5237"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])), // Secret key
            ValidAudiences = new List<string>(){
                builder.Configuration["Jwt:Issuer"]
            }
        };
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validated for {context.Principal.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        Console.WriteLine($"Authorization: {authHeader}");
    }
    else
    {
        Console.WriteLine("Authorization header not found.");
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapCarsEndPoints();
app.MapCompanyEndpoints();
app.MapModelEndpoints();
app.MapColorEndpoints();
app.MapReviewEndpoints();
app.MapUserEndpoints();

await app.MigrateDbAsync();

app.Run();
