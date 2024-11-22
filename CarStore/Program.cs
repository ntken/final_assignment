using CarStore.Data;
using CarStore.EndPoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Concurrent;
using CarStore.Services;

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
var TokenBlacklist = new ConcurrentBag<string>();
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
        options.ClaimsIssuer = jwtIssuer;
        options.Audience = jwtIssuer;
        options.Authority = jwtIssuer;
        options.UseSecurityTokenValidators = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = false,
            ValidateIssuer = false,
            ValidIssuer = jwtIssuer, // "localhost"
            ValidateAudience = false,
            ValidAudience = jwtIssuer, // "localhost"
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)), // Secret key
            ValidAudiences = new List<string>() { jwtIssuer },
            ValidateTokenReplay = true, // Kích hoạt kiểm tra token bị thu hồi
            TokenReplayValidator = (expirationTime, securityToken, validationParameters) =>
            {
                // Lấy token từ securityToken
                var token = securityToken as string;
                if (token != null && TokenBlacklist.Contains(token))
                {
                    Console.WriteLine($"Token is revoked: {token}");
                    return false;
                }
                return true;
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
                var identityName = context.Principal?.Identity?.Name;
                if (!string.IsNullOrEmpty(identityName))
                {
                    Console.WriteLine($"Token validated for {identityName}");
                }
                else
                {
                    Console.WriteLine("Token validated, but no identity name found.");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenBlacklistService>();

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
