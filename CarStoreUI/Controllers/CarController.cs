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

    // Action to display car details
    [HttpGet("/Car/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        var reviews = await _carService.GetReviewsAsync(id);
        ViewBag.Reviews = reviews;
        return View(car);
    }

    [Route("Car/ByFilter/{filterType}/{filterValue}")]
    public async Task<IActionResult> ByFilter(string filterType, string filterValue)
    {
        var cars = await _carService.GetCarsByFilterAsync(filterType, filterValue);
        return View("Index", cars);
    }
}
