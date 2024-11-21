using System;
using CarStore.Data;
using CarStore.Mapping;
using CarShop.Shared.Models;
using CarStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarStore.EndPoints;

public static class ModelEndpoints
{
    public static RouteGroupBuilder MapModelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("models");

        group.MapGet("/", async (CarStoreContext dbContext) =>
            await dbContext.Models
            .Select(model => model.ToDto())
            .AsNoTracking()
            .ToListAsync());

        // GET: Get a model by ID
        group.MapGet("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var model = await dbContext.Models.FindAsync(id);
            if (model == null)
            {
                return Results.NotFound($"Model with ID {id} not found.");
            }

            return Results.Ok(model.ToDto());
        });

        // POST: Create a new model
        group.MapPost("/", async (CreateItemDto createModelDto, CarStoreContext dbContext) =>
        {
            var model = new Model
            {
                Name = createModelDto.Name
            };

            dbContext.Models.Add(model);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/models/{model.Id}", model.ToDto());
        });

        // PUT: Update an existing model
        group.MapPut("/{id}", async (int id, UpdateItemDto updateModelDto, CarStoreContext dbContext) =>
        {
            var existingModel = await dbContext.Models.FindAsync(id);
            if (existingModel == null)
            {
                return Results.NotFound($"Model with ID {id} not found.");
            }

            existingModel.Name = updateModelDto.Name;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE: Delete a model
        group.MapDelete("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var existingModel = await dbContext.Models.FindAsync(id);
            if (existingModel == null)
            {
                return Results.NotFound($"Model with ID {id} not found.");
            }

            dbContext.Models.Remove(existingModel);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}
