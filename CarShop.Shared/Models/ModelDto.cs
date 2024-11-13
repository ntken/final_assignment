using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class ModelDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Model Name is required")]
    public string Name { get; set; }

    public ModelDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
