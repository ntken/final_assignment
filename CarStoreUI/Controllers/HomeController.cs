using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarShop.Shared.Models;
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
        var companies = await _carService.GetCompaniesAsync();
        var models = await _carService.GetModelsAsync();
        var colors = await _carService.GetColorsAsync();
        var featuredProducts = await _carService.GetCarsAsync();

        ViewBag.Companies = companies;
        ViewBag.Models = models;
        ViewBag.Colors = colors;

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
