using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarShop.Shared.Models;

public class CarService
{
    private readonly HttpClient _httpClient;

    public CarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CarDto>> GetCarsAsync()
    {
        // Step 1: Fetch the list of cars from the API endpoint
        var response = await _httpClient.GetAsync("http://localhost:5237/cars");
        response.EnsureSuccessStatusCode();
        var cars = await response.Content.ReadFromJsonAsync<List<CarDto>>() ?? new List<CarDto>();

        // Step 2: Fetch the lists of companies, models, and colors
        var companies = await GetCompaniesAsync();
        var models = await GetModelsAsync();
        var colors = await GetColorsAsync();

        // Step 3: Assign names to each car based on IDs
        foreach (var car in cars)
        {
            car.CompanyName = companies.FirstOrDefault(c => c.Id == car.CompanyId)?.Name ?? "Unknown";
            car.ModelName = models.FirstOrDefault(m => m.Id == car.ModelId)?.Name ?? "Unknown";
            car.ColorName = colors.FirstOrDefault(col => col.Id == car.ColorId)?.Name ?? "Unknown";
            // Console.WriteLine($"Car ID: {car.Id}, CompanyName: {car.CompanyName}, ModelName: {car.ModelName}, ColorName: {car.ColorName}");
        }
        return cars;
    }

    public async Task<CarDto?> GetCarByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5237/cars/{id}");
        if (response.IsSuccessStatusCode)
        {
            var car = await response.Content.ReadFromJsonAsync<CarDto>();

            if (car != null)
            {
                // Lấy thêm thông tin Company, Model, và Color
                var companies = await GetCompaniesAsync();
                var models = await GetModelsAsync();
                var colors = await GetColorsAsync();

                // Thay vì so sánh với string, chúng ta so sánh Id để lấy tên
                car.CompanyName = companies.FirstOrDefault(c => c.Id == car.CompanyId)?.Name ?? "Unknown";
                car.ModelName = models.FirstOrDefault(m => m.Id == car.ModelId)?.Name ?? "Unknown";
                car.ColorName = colors.FirstOrDefault(col => col.Id == car.ColorId)?.Name ?? "Unknown";
            }

            return car;
        }

        return null;
    }

    public async Task CreateCarAsync(CarDto car)
    {
        await _httpClient.PostAsJsonAsync("http://localhost:5237/cars", car);
    }

    public async Task UpdateCarAsync(int id, CarDto car)
    {
        await _httpClient.PutAsJsonAsync($"http://localhost:5237/cars/{id}", car);
    }

    public async Task DeleteCarAsync(int id)
    {
        await _httpClient.DeleteAsync($"http://localhost:5237/cars/{id}");
    }

    public async Task<List<CarDto>> GetCarsByFilterAsync(string filterType, string filterValue)
    {
        // Construct the URL based on the filter type and value
        string url = $"http://localhost:5237/cars?{filterType}={filterValue}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var cars = await response.Content.ReadFromJsonAsync<List<CarDto>>() ?? new List<CarDto>();

        // Fetch additional data for mapping names
        var companies = await GetCompaniesAsync();
        var models = await GetModelsAsync();
        var colors = await GetColorsAsync();

        // Assign company, model, and color names to each car
        foreach (var car in cars)
        {
            car.CompanyName = companies.FirstOrDefault(c => c.Id == car.CompanyId)?.Name ?? "Unknown";
            car.ModelName = models.FirstOrDefault(m => m.Id == car.ModelId)?.Name ?? "Unknown";
            car.ColorName = colors.FirstOrDefault(col => col.Id == car.ColorId)?.Name ?? "Unknown";
        }

        return cars;
    }

    public async Task<List<CompanyDto>> GetCompaniesAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5237/companies");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CompanyDto>>() ?? new List<CompanyDto>();
    }

    public async Task<List<ModelDto>> GetModelsAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5237/models");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ModelDto>>() ?? new List<ModelDto>();
    }

    public async Task<List<ColorDto>> GetColorsAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5237/colors");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ColorDto>>() ?? new List<ColorDto>();
    }

    public async Task<List<ReviewDto>> GetReviewsAsync(int carId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5237/reviews/{carId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ReviewDto>>() ?? new List<ReviewDto>();
    }

    public async Task AddReviewAsync(ReviewDto reviewDto)
    {
        await _httpClient.PostAsJsonAsync("http://localhost:5237/reviews", reviewDto);
    }

}
