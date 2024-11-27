using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using CarShop.Shared.Models;

public class CarService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            //Console.WriteLine($"Using token: {token}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
    public CarService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<CarDto>> GetCarsAsync()
    {
        AddAuthorizationHeader();
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
        AddAuthorizationHeader();
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
        AddAuthorizationHeader();
        await _httpClient.PostAsJsonAsync("http://localhost:5237/cars", car);
    }

    public async Task UpdateCarAsync(int id, CarDto car)
    {
        AddAuthorizationHeader();
        await _httpClient.PutAsJsonAsync($"http://localhost:5237/cars/{id}", car);
    }

    public async Task DeleteCarAsync(int id)
    {
        AddAuthorizationHeader();
        await _httpClient.DeleteAsync($"http://localhost:5237/cars/{id}");
    }

    public async Task<List<CarDto>> GetCarsByFilterAsync(string filterType, string filterValue)
    {
        AddAuthorizationHeader();
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
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("http://localhost:5237/companies");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CompanyDto>>() ?? new List<CompanyDto>();
    }

    public async Task<List<ModelDto>> GetModelsAsync()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("http://localhost:5237/models");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ModelDto>>() ?? new List<ModelDto>();
    }

    public async Task<List<ColorDto>> GetColorsAsync()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync("http://localhost:5237/colors");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ColorDto>>() ?? new List<ColorDto>();
    }

    public async Task<List<ReviewDto>> GetReviewsAsync(int carId)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync($"http://localhost:5237/reviews/{carId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ReviewDto>>() ?? new List<ReviewDto>();
    }

    public async Task AddReviewAsync(ReviewDto reviewDto)
    {
        AddAuthorizationHeader();
        await _httpClient.PostAsJsonAsync("http://localhost:5237/reviews", reviewDto);
    }

    public async Task<double?> GetAverageRatingAsync(int carId)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync($"http://localhost:5237/reviews/average-rating/{carId}");
        response.EnsureSuccessStatusCode();

        var averageRating = await response.Content.ReadFromJsonAsync<double?>();
        return averageRating;
    }
}
