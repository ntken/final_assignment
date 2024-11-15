using CarStore.Data;
using CarStore.Entities;
using CarShop.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CarStore.EndPoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");

        // POST /users/register
        group.MapPost("/register", async (RegisterUserDto registerDto, CarStoreContext dbContext) =>
        {
            // Kiểm tra nếu email đã tồn tại
            var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                return Results.Conflict("Email already registered.");
            }

            // Mã hóa mật khẩu
            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)));

            // Tạo người dùng mới
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
        group.MapPost("/login", async (LoginUserDto loginDto, CarStoreContext dbContext) =>
        {
            // Kiểm tra xem người dùng có tồn tại không
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Results.Json(new { message = "Invalid email or password." }, statusCode: 401);
            }

            // Xác minh mật khẩu
            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)));

            if (user.PasswordHash != passwordHash)
            {
                return Results.Json(new { message = "Invalid email or password." }, statusCode: 401);
            }

            return Results.Ok("Login successful.");
        });

        return group;
    }
}
