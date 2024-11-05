using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public record class CarSummaryDto (
    int Id, 
    string Company,
    string Model,
    string Color,
    decimal Price,
    DateOnly ReleasedDate,
    string? Description,
    string? Image
);
