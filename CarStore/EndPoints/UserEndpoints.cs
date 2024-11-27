using CarStore.Data;
using CarStore.Entities;
using CarShop.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using CarStore.Utils;
using CarStore.Services;

namespace CarStore.EndPoints;

public static class UserEndpoints
{
    private static readonly List<string> TokenBlacklist = new();
    private static string GenerateJwtToken(User user, string key, string issuer)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

        var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Role", user.Role),
            };

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(60)),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        //Console.WriteLine($"GenerateJwtToken: Issuer = {issuer}");
        //Console.WriteLine($"Token for user {user.Email} with role {user.Role}");

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");

        // POST /users/register
        group.MapPost("/register", async (RegisterUserDto registerDto, CarStoreContext dbContext) =>
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                return Results.Conflict("Email already registered.");
            }

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)));

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                FullName = registerDto.FullName
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.Ok("User registered successfully.");
        });

        // POST /users/login
        group.MapPost("/login", async (LoginUserDto loginDto, CarStoreContext dbContext, IConfiguration configuration) =>
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Results.Json(new { message = "Invalid credentials." }, statusCode: 401);
            }

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)));

            if (user.PasswordHash != passwordHash)
            {
                return Results.Json(new { message = "Invalid credentials." }, statusCode: 401);
            }

            var jwtKey = configuration["Jwt:Key"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
            {
                throw new InvalidOperationException("JWT settings are not configured.");
            }
            var token = GenerateJwtToken(user, jwtKey, jwtIssuer);

            return Results.Ok(new { token });
        });

        // POST /users/logout
        group.MapPost("/logout", async (HttpContext context, TokenBlacklistService tokenBlacklistService) =>
        {
            // Lưu token vào danh sách thu hồi (Blacklist)
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return Results.BadRequest("Authorization header is missing.");
            }

            var token = authHeader.ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Results.BadRequest("Token is missing.");
            }

            // Lưu token vào danh sách thu hồi
            tokenBlacklistService.AddToken(token);

            await Task.CompletedTask; // Thêm lệnh này để loại bỏ cảnh báo
            return Results.Ok(new { message = "Logged out successfully" });
        });

        // GET /users
        group.MapGet("/", [Authorize] async (HttpContext httpContext, CarStoreContext dbContext) =>
        {
            if (!AuthorizationUtils.IsAdmin(httpContext))
            {
                return Results.Forbid();
            }
            var users = await dbContext.Users
                .Select(u => new { u.Id, u.Email, u.FullName, u.Role })
                .ToListAsync();
            return Results.Ok(users);
        });

        // DELETE /users/{id}
        group.MapDelete("/{id}", [Authorize] async (HttpContext httpContext, int id, CarStoreContext dbContext) =>
        {
            if (!AuthorizationUtils.IsAdmin(httpContext))
            {
                return Results.Forbid();
            }
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });        

        return group;
    }
}
