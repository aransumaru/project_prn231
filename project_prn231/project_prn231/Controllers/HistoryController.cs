using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;

namespace project_prn231.Controllers
{
    [Route("History")]
    public class HistoryController : Controller
    {
        public HistoryController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlExam = "https://localhost:7272/api/Exam/User";
        public async Task<IActionResult> History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            using (HttpResponseMessage res = await _httpClient.GetAsync($"{urlExam}/{userId}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();
                    List<Exam> exams = JsonConvert.DeserializeObject<List<Exam>>(result);
                    return View(exams);
                }
                else
                {
                    return NotFound($"Không tìm thấy lịch sử bài thi.");

                }
            }
        }
    }
}

