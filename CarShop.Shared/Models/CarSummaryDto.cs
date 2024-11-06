using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public record class CarSummaryDto (
    int Id, 
    int CompanyId,
    int ModelId,
    int ColorId,
    string Company,
    string Model,
    string Color,
    decimal Price,
    DateOnly ReleasedDate,
    string? Description,
    string? Image
);
