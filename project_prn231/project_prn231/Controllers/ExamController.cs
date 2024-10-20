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
            // Lấy PkUserId từ session
            int? userId = HttpContext.Session.GetInt32("UserId"); // đảm bảo bạn đã thiết lập session này
            if (userId == null)
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            // Tính số điểm
            int point = 0;

            // Kiểm tra từng answer xem có đúng không
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

            // Tạo đối tượng Exam
            var exam = new Exam
            {
                PkUserId = userId,
                PkCategoryId = categoryId,
                ExamDate = DateTime.Now,
                Point = point
            };

            // Gửi yêu cầu POST đến API để lưu exam
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