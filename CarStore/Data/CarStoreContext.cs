using System;
using System.Data.Common;
using CarStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Data;

public class CarStoreContext(DbContextOptions<CarStoreContext> options) 
: DbContext(options)
{
    public DbSet<Car> Cars => Set<Car>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Model> Models => Set<Model>();

    public DbSet<Color> Colors => Set<Color>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>().HasData(
            new { Id = 1, Name = "Toyota"},
            new { Id = 2, Name = "Mercedes"},
            new { Id = 3, Name = "Porches"},
            new { Id = 4, Name = "Mazda"},
            new { Id = 5, Name = "BMW"}
        );
        modelBuilder.Entity<Model>().HasData(
            new { Id = 1, Name = "Camry v3"},
            new { Id = 2, Name = "GMJ"},
            new { Id = 3, Name = "Cayan"},
            new { Id = 4, Name = "CX6"},
            new { Id = 5, Name = "i8 v3"}
        );
        modelBuilder.Entity<Color>().HasData(
            new { Id = 1, Name = "Red"},
            new { Id = 2, Name = "Black"},
            new { Id = 3, Name = "White"},
            new { Id = 4, Name = "Green Midnight"},
            new { Id = 5, Name = "Golden"}
        );
    }
}
