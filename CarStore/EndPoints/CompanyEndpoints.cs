using System;
using CarStore.Data;
using CarStore.Mapping;
using CarStore.Entities;
using CarShop.Shared.Models;
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

        // GET: Get a company by ID
        group.MapGet("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var company = await dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                return Results.NotFound($"Company with ID {id} not found.");
            }

            return Results.Ok(company.ToDto());
        });

        // POST: Create a new company
        group.MapPost("/", async (CreateItemDto createCompanyDto, CarStoreContext dbContext) =>
        {
            var company = new Company
            {
                Name = createCompanyDto.Name
            };

            dbContext.Companies.Add(company);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/companies/{company.Id}", company.ToDto());
        });

        // PUT: Update an existing company
        group.MapPut("/{id}", async (int id, UpdateItemDto updateCompanyDto, CarStoreContext dbContext) =>
        {
            var existingCompany = await dbContext.Companies.FindAsync(id);
            if (existingCompany == null)
            {
                return Results.NotFound($"Company with ID {id} not found.");
            }

            existingCompany.Name = updateCompanyDto.Name;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE: Delete a company
        group.MapDelete("/{id}", async (int id, CarStoreContext dbContext) =>
        {
            var existingCompany = await dbContext.Companies.FindAsync(id);
            if (existingCompany == null)
            {
                return Results.NotFound($"Company with ID {id} not found.");
            }

            dbContext.Companies.Remove(existingCompany);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}
