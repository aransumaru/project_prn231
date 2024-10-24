using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;

namespace project_prn231.Controllers
{

    [Route("Question")]
    public class QuestionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string urlQuestion = "https://localhost:7272/api/Question";


        public QuestionController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<IActionResult> Question(int categoryId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            using (HttpClient client = _httpClient)
            {
                using (HttpResponseMessage res = await client.GetAsync($"{urlQuestion}/GetByCategory?categoryId={categoryId}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        string result = await res.Content.ReadAsStringAsync();
                        List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(result);

                        return View(questions);
                    }
                    else
                    {
                        return NotFound("Không tìm thấy câu hỏi.");
                    }
                }
            }
        }


    }
}
