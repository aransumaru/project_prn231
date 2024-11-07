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
        private readonly string urlUserAnswer = "https://localhost:7272/api/UserAnswer";
        private readonly string urlQuestion = "https://localhost:7272/api/Question";
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

        [HttpGet("History/Detail")]
        public async Task<IActionResult> Detail(int examId, int categoryId)
        {
            using (HttpResponseMessage userAnswerRes = await _httpClient.GetAsync($"{urlUserAnswer}/Exam/{examId}"))
            {
                if (!userAnswerRes.IsSuccessStatusCode)
                {
                    return NotFound("Không tìm thấy chi tiết bài thi.");
                }

                string userAnswerResult = await userAnswerRes.Content.ReadAsStringAsync();
                List<AnswerDetailViewModel> userAnswers = JsonConvert.DeserializeObject<List<AnswerDetailViewModel>>(userAnswerResult);

                using (HttpResponseMessage questionRes = await _httpClient.GetAsync($"{urlQuestion}/GetByCategory?categoryId={categoryId}"))
                {
                    if (!questionRes.IsSuccessStatusCode)
                    {
                        return NotFound("Không tìm thấy dữ liệu câu hỏi.");
                    }

                    string questionResult = await questionRes.Content.ReadAsStringAsync();
                    List<QuestionViewModel> questions = JsonConvert.DeserializeObject<List<QuestionViewModel>>(questionResult);

                    var answerDetails = questions.SelectMany(q => q.Answers.Select(a => new AnswerDetailViewModel
                    {
                        QuestionText = q.QuestionText,
                        QuestionImage = q.QuestionImage,
                        AnswerText = a.AnswerText,
                        AnswerImage = a.AnswerImage,
                        IsCorrect = a.IsCorrect,
                        IsSelected = userAnswers.Any(ua => ua.QuestionText == q.QuestionText && ua.AnswerText == a.AnswerText && ua.IsSelected)
                    })).ToList();

                    return View("Detail", answerDetails);
                }
            }
        }

    }
}

