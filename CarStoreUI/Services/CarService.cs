using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarStoreUI.Models;

public class CarService
{
    private readonly HttpClient _httpClient;

    public CarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CarDto>> GetCarsAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5237/cars");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CarDto>>() ?? new List<CarDto>();
    }

    public async Task<CarDto?> GetCarByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5237/cars/{id}");
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<CarDto>() : null;
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
        var response = await _httpClient.GetAsync($"http://localhost:5237/cars?{filterType}={filterValue}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CarDto>>() ?? new List<CarDto>();
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
}
