using CarStore.Data;
using CarStore.Entities;
using CarStore.Mapping;
using CarShop.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStore.EndPoints;

public static class CarsEndpoints
{
    const string GetCarEndPointName = "GetCar";

    public static RouteGroupBuilder MapCarsEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("cars")
                .WithParameterValidation();
        //GET cars
        group.MapGet("/", async (CarStoreContext dbContext, string? company, string? model, string? color) =>
        {
            var query = dbContext.Cars
                .Include(car => car.Company)
                .Include(car => car.Model)
                .Include(car => car.Color)
                .AsQueryable();

            // Áp dụng bộ lọc nếu có tham số truy vấn
            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(car => car.Company != null && car.Company.Name.ToLower() == company.ToLower());
            }

            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(car => car.Model != null && car.Model.Name.ToLower() == model.ToLower());
            }

            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(car => car.Color != null && car.Color.Name.ToLower() == color.ToLower());
            }

            var cars = await query
                .Select(car => car.ToCarSummaryDto())
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(cars);
        });

        //GET /cars/1
        group.MapGet("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            Car? car = await dbContext.Cars.FindAsync(id);

            return car is null ? Results.NotFound() : Results.Ok(car.ToCarDetailsDto());
        })
        .WithName(GetCarEndPointName);

        //POST /cars
        group.MapPost("/", async (CreateCarDto newCar, CarStoreContext dbContext) =>
        {
            Car car = newCar.ToEntity();

            dbContext.Cars.Add(car);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                GetCarEndPointName, 
                new { id = car.Id }, 
                car.ToCarDetailsDto());
        });

        //PUT /cars
        group.MapPut("/{id}", async (int id, UpdateCarDto updateCar, CarStoreContext dbContext) =>
        {
            var existingCar = await dbContext.Cars.FindAsync(id);

            if (existingCar is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingCar)
            .CurrentValues
            .SetValues(updateCar.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /cars/1
        group.MapDelete("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            await dbContext.Cars
            .Where(car => car.Id == id)
            .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
