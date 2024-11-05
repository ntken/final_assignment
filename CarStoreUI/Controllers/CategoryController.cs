using CarStoreUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarStoreUI.Controllers;

public class CategoryController : Controller
{
    public IActionResult Index()
    {
        var categories = new List<Category>
        {
            new Category {Id = 1, Name = "company"},
            new Category {Id = 2, Name = "model"},
            new Category {Id = 3, Name = "color"}
        };

        return View(categories);
    }
}
