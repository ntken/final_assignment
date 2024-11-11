using CarStore.Data;
using CarStore.Entities;
using CarShop.Shared.Models;
using CarStore.Mapping;
using Microsoft.EntityFrameworkCore;

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
            group.MapPost("/", async (ReviewDto reviewDto, CarStoreContext dbContext) =>
            {
                var review = reviewDto.ToEntity();
                review.ReviewDate = DateTime.Now;
                dbContext.Reviews.Add(review);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/reviews/{review.Id}", review.ToDto());
            });

            return group;
        }
    }
}
