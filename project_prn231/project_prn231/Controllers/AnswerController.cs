using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using project_prn231.Models;

namespace project_prn231.Controllers
{
    
    public class AnswerController : Controller
    {
        public AnswerController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string url = "https://localhost:7272/api/Answer";
        public async Task<IActionResult> Index()
        {
            using (HttpClient client = _httpClient)
            {
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        using (HttpContent content = res.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            List<Answer> answers = JsonConvert.DeserializeObject<List<Answer>>(result);
                            return View(answers);
                        }
                    }
                    else
                    {
                        return NotFound("Không tìm thấy câu trả lời.");
                    }
                }
            }
        }
    }
}
