using Microsoft.AspNetCore.Mvc;
using CarStoreUI.Services;
using System.Threading.Tasks;

public class CarController : Controller
{
    private readonly CarService _carService;

    public CarController(CarService carService)
    {
        _carService = carService;
    }

    public async Task<IActionResult> Index()
    {
        // Lấy danh sách xe làm Featured Products
        var featuredProducts = await _carService.GetCarsAsync();
        ViewBag.FeaturedProducts = featuredProducts;

        return View();
    }

    // public async Task<IActionResult> Index()
    // {
    //     var cars = await _carService.GetCarsAsync();
    //     return View(cars);
    // }

    public async Task<IActionResult> ByFilter(string filterType, string filterValue)
    {
        var cars = await _carService.GetCarsByFilterAsync(filterType, filterValue);
        return View("Index", cars);
    }
}
