using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class CarDto
{
    public int Id { get; set; }
    public required string Company { get; set; }
    public required string Model { get; set; }
    public required string Color { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleasedDate { get; set; }
}
