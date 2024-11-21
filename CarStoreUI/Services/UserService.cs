using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarShop.Shared.Models;
using System.Text.Json;

namespace CarStoreUI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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
            try
            {
                // Gửi request đến API
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5237/users/login", user);

                // Kiểm tra phản hồi HTTP
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung JSON từ phản hồi
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");

                    try
                    {
                        // Phân tích JSON để lấy token
                        var jsonDocument = JsonDocument.Parse(responseContent);
                        if (jsonDocument.RootElement.TryGetProperty("token", out var tokenElement))
                        {
                            string? token = tokenElement.GetString();
                            Console.WriteLine($"Token received: {token}");

                            if (!string.IsNullOrEmpty(token))
                            {
                                // Lưu token vào session
                                _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", token);
                                return "Login successful.";
                            }
                            else
                            {
                                return "Token is empty or null.";
                            }
                        }
                        else
                        {
                            return "Token not found in response.";
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                        return "Failed to parse JSON response.";
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "Invalid email or password.";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : "An unexpected error occurred.";
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return "Unable to reach the server.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return "An unexpected error occurred.";
            }
        }
    }
}
