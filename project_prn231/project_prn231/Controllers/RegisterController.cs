using Microsoft.AspNetCore.Mvc;
using project_prn231.Models;
using System;

namespace project_prn231.Controllers
{
    [Route("Register")]

    public class RegisterController : Controller
    {
        public RegisterController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlUser = "https://localhost:7272/api/User/login";

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto); // Trả về view với thông tin nhập
            }

            // Gửi yêu cầu POST đến API để đăng ký
            var response = await _httpClient.PostAsJsonAsync(urlUser, userDto);

            if (response.IsSuccessStatusCode)
            {
                // Đăng ký thành công, có thể điều hướng đến trang đăng nhập
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đăng ký không thành công. Vui lòng thử lại.");
                return View(userDto); // Trả về view với thông báo lỗi
            }
        }
    }
}
