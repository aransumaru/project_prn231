using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using project_prn231.Models;

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
        private readonly string urlUser = "https://localhost:7272/api/User";

        // GET: Register
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Trả về view đăng ký
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] User user)
        {

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpResponseMessage res = await _httpClient.PostAsync(urlUser, content))
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    string errorMessage = await res.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = "Đăng ký không thành công: " + errorMessage;
                    return View("Index", user);
                }
            }
        }

    }
}
