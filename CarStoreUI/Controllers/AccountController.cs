using Microsoft.AspNetCore.Mvc;
using CarShop.Shared.Models;
using CarStoreUI.Services;
using System.Threading.Tasks;
using System.Text.Json;

namespace CarStoreUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                // Gọi UserService để đăng ký người dùng
                var result = await _userService.RegisterUserAsync(model);

                if (result == "User registered successfully.")
                {
                    // Đăng ký thành công, lưu trạng thái đăng nhập vào Session
                    HttpContext.Session.SetString("UserEmail", model.Email);
                    HttpContext.Session.SetString("IsLoggedIn", "true");

                    // Chuyển hướng với query parameter "registration=success" để hiển thị thông báo
                    return RedirectToAction("Register", new { registration = "success" });
                }
                else if (result == "Email already exists.")
                {
                    // Thông báo email trùng
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }
                else
                {
                    // Thông báo lỗi chung khác
                    ModelState.AddModelError(string.Empty, result);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto model)
        {
            if (ModelState.IsValid)
            {
                // Gọi UserService để xử lý login
                var result = await _userService.LoginUserAsync(model);

                if (result == "Login successful.")
                {
                    // Chuyển hướng về trang chủ nếu login thành công
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Thêm lỗi vào ModelState để hiển thị trên giao diện
                    ModelState.AddModelError(string.Empty, result);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Session.GetString("JwtToken");

            // Gọi API /logout
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync("http://localhost:5237/users/logout", null);
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home", new { logout = "failed" });
                }
            }

            // Xóa token và trạng thái Session
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("IsLoggedIn");

            return RedirectToAction("Index", "Home");
        }
    }
}
