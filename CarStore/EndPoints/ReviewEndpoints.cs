using CarStore.Data;
using CarStore.Entities;
using CarShop.Shared.Models;
using CarStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarStore.Services;

namespace CarStore.EndPoints
{
    public static class ReviewEndpoints
    {
        public static RouteGroupBuilder MapReviewEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("reviews");

            // GET: Get reviews for a specific car
            group.MapGet("/{carId}", async (int carId, CarStoreContext dbContext) =>
            {
                var reviews = await dbContext.Reviews
                    .Where(r => r.CarId == carId)
                    .Select(r => r.ToDto())
                    .ToListAsync();
                return Results.Ok(reviews);
            });

            // POST: Add a new review
            group.MapPost("/", [Authorize] async (HttpContext context, ReviewDto reviewDto, CarStoreContext dbContext, TokenBlacklistService tokenBlacklistService) =>
            {
                if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var token = authHeader.ToString().Replace("Bearer ", "");
                    if (tokenBlacklistService.IsTokenBlacklisted(token))
                    {
                        return Results.Unauthorized(); // Token đã bị thu hồi
                    }
                }
                var review = reviewDto.ToEntity();
                review.ReviewDate = DateTime.Now;
                dbContext.Reviews.Add(review);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/reviews/{review.Id}", review.ToDto());
            });

            group.MapGet("/average-rating/{carId}", async (int carId, CarStoreContext dbContext) =>
{
    var averageRating = await dbContext.Reviews
        .Where(r => r.CarId == carId)
        .AverageAsync(r => (double?)r.Rating) ?? 0;

    return Results.Ok(averageRating);
});


            return group;
        }
    }
}
