using System;
using CarStore.Data;
using CarStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CarStore.EndPoints;

public static class CompanyEndpoints
{
    public static RouteGroupBuilder MapCompanyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("companies");

        group.MapGet("/", async (CarStoreContext dbContext) =>
            await dbContext.Companies
            .Select(company => company.ToDto())
            .AsNoTracking()
            .ToListAsync());
            
        return group;
    }
}
