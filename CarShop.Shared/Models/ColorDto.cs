using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class ColorDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Color Name is required")]
    public string Name { get; set; }

    public ColorDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
