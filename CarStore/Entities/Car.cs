namespace CarStore.Entities;

public class Car
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public int ModelId { get; set; }
    public Model? Model { get; set; }
    public int ColorId { get; set; }
    public Color? Color { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleasedDate { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
