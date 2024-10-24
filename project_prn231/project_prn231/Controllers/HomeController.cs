using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;
using System.Diagnostics;
using System.Net.Http;

namespace project_prn231.Controllers
{
    [Route("")]

    public class HomeController : Controller
    {
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlCategory = "https://localhost:7272/api/Category";

        public async Task<IActionResult> Index()
        {


            using (HttpClient client = _httpClient)
            {
                using (HttpResponseMessage res = await client.GetAsync(urlCategory))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        using (HttpContent content = res.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(result);
                            return View(categories);
                        }
                    }
                    else
                    {
                        return NotFound("Không tìm thấy danh mục.");
                    }
                }
            }
        }

    }
}
