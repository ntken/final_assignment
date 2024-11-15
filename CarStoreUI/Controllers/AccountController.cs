using Microsoft.AspNetCore.Mvc;
using CarShop.Shared.Models;
using CarStoreUI.Services;
using System.Threading.Tasks;

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
                    // Đăng ký thành công, chuyển hướng với query parameter "registration=success"
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
                var result = await _userService.LoginUserAsync(model);
                if (result == "Login successful.")
                {
                    // Đăng nhập thành công, chuyển hướng với query parameter "login=success"
                    return RedirectToAction("Login", new { login = "success" });
                }
                else
                {
                    // Đăng nhập thất bại, chuyển hướng với query parameter "login=failed"
                    return RedirectToAction("Login", new { login = "failed" });
                }
            }
            return View(model);
        }

    }
}
