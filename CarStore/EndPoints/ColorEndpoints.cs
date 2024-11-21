using System;
using CarStore.Data;
using CarStore.Mapping;
using CarStore.Entities;
using CarShop.Shared.Models;
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

        // GET /colors/{id}: Lấy chi tiết một màu
        group.MapGet("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var color = await dbContext.Colors.FindAsync(id);
            return color is null ? Results.NotFound("Color not found.") : Results.Ok(color.ToDto());
        });

        // POST /colors: Thêm mới một màu
        group.MapPost("/", async (CreateItemDto newColorDto, CarStoreContext dbContext) =>
{
    var newColor = new Color { Name = newColorDto.Name };
    dbContext.Colors.Add(newColor);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/colors/{newColor.Id}", newColor.ToDto());
});

        // PUT /colors/{id}: Cập nhật một màu
        group.MapPut("/{id}", async (int id, UpdateItemDto updateColorDto, CarStoreContext dbContext) =>
{
    // Tìm màu sắc có ID được cung cấp
    var existingColor = await dbContext.Colors.FindAsync(id);
    if (existingColor is null)
    {
        return Results.NotFound($"Color with ID {id} not found.");
    }

    // Cập nhật thông tin màu sắc
    existingColor.Name = updateColorDto.Name;
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

        // DELETE /colors/{id}: Xóa một màu
        group.MapDelete("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var color = await dbContext.Colors.FindAsync(id);
            if (color is null)
            {
                return Results.NotFound("Color not found.");
            }

            dbContext.Colors.Remove(color);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}
