using System;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarStoreContext>();
        await dbContext.Database.MigrateAsync();
    }
}
