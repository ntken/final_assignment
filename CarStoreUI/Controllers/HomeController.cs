using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarStoreUI.Models;
using CarStoreUI.Services;

namespace CarStoreUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CarService _carService;

    public HomeController(ILogger<HomeController> logger, CarService carService)
    {
        _logger = logger;
        _carService = carService;
    }

    public async Task<IActionResult> Index()
    {
        // Get data from the API using CarService
        var companies = await _carService.GetCompaniesAsync();
        var models = await _carService.GetModelsAsync();
        var colors = await _carService.GetColorsAsync();
        var featuredProducts = await _carService.GetCarsAsync();

        // Set data to ViewBag (Use the correct DTOs)
        ViewBag.Companies = companies;  // Companies is a list of CompanyDto
        ViewBag.Models = models;        // Models is a list of ModelDto
        ViewBag.Colors = colors;        // Colors is a list of ColorDto

        // Only take the first 3 cars for featured products
        ViewBag.FeaturedProducts = featuredProducts.Take(3).ToList();

        return View(featuredProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
