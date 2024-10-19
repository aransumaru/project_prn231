using Microsoft.AspNetCore.Mvc;
using project_prn231.Models;

namespace project_prn231.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly string url = "https://localhost:7272/api/User/login";
        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private readonly HttpClient _httpClient;
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync(url, loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserDto>(); // Đọc dữ liệu trả về
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                return RedirectToAction("Index", "Home"); // Điều hướng đến trang chính sau khi đăng nhập thành công
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không đúng.");
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
