using System;
using CarStore.Data;
using CarStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CarStore.EndPoints;

public static class ColorEndpoints
{
    public static RouteGroupBuilder MapColorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("colors");

        group.MapGet("/", async (CarStoreContext dbContext) =>
            await dbContext.Colors
            .Select(color => color.ToDto())
            .AsNoTracking()
            .ToListAsync());
            
        return group;
    }
}
