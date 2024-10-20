using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using project_prn231.Models;
namespace project_prn231.Controllers
{
    [Route("Exam")]
    public class ExamController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string urlExam = "https://localhost:7272/api/Exam";
        private readonly string urlAnswer = "https://localhost:7272/api/Answer";

        public ExamController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<IActionResult> Submit(int categoryId, List<int> selectedAnswers)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            int point = 0;

            foreach (var answerId in selectedAnswers)
            {
                using (HttpResponseMessage res = await _httpClient.GetAsync($"{urlAnswer}/{answerId}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var answerJson = await res.Content.ReadAsStringAsync();
                        var answer = JsonConvert.DeserializeObject<Answer>(answerJson);
                        if (answer != null && answer.IsCorrect == true)
                        {
                            point++;
                        }
                    }
                }
            }

            var exam = new Exam
            {
                PkUserId = userId,
                PkCategoryId = categoryId,
                ExamDate = DateTime.Now,
                Point = point
            };

            using (HttpResponseMessage res = await _httpClient.PostAsJsonAsync(urlExam, exam))
            {
                if (res.IsSuccessStatusCode)
                {
                    return View("Exam", exam);
                }
                else
                {
                    return BadRequest("Có lỗi khi lưu thông tin bài thi.");
                }
            }
        }

    }
}