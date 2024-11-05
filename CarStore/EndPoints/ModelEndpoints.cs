using System;
using CarStore.Data;
using CarStore.Mapping;
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
            
        return group;
    }
}
