using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarShop.Shared.Models;

namespace CarStoreUI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5237/users/register", user);

            if (response.IsSuccessStatusCode)
            {
                return "User registered successfully.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Xử lý mã trạng thái 409 (Conflict) khi email đã tồn tại
                return "Email already exists.";
            }
            else
            {
                // Xử lý các lỗi khác và trả về thông báo lỗi chung
                var errorMessage = await response.Content.ReadAsStringAsync();
                return !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : "An error occurred while registering the user.";
            }
        }

        public async Task<string> LoginUserAsync(LoginUserDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5237/users/login", user);

            if (response.IsSuccessStatusCode)
            {
                return "Login successful.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "Invalid email or password.";
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : "An error occurred during login.";
            }
        }

    }
}
