using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class CarDto
{
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public int ColorId { get; set; }
        public string? ColorName { get; set; }
        public decimal Price { get; set; }
        public DateOnly ReleasedDate { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
        public double? AverageRating { get; set; }
}
