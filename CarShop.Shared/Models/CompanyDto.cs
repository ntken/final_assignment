using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models;

public class CompanyDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Company Name is required")]
    public string Name { get; set; }

    public CompanyDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
