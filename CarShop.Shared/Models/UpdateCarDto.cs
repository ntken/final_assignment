using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public record class UpdateCarDto(
    int CompanyId,
    int ModelId,
    int ColorId,
    [Required][Range(1, 2000000)]decimal Price,
    DateOnly ReleasedDate,
    string? Description,
    string? Image
);
