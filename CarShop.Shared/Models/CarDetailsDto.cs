using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public record class CarDetailsDto (
    int Id, 
    int CompanyId,
    int ModelId,
    int ColorId,
    decimal Price,
    DateOnly ReleasedDate,
    string? Description,
    string? Image
);
