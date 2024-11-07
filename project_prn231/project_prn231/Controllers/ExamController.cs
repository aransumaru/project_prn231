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

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            if (userId == null)
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            int point = 0;
            List<(int? questionId, int answerId)> userAnswers = new List<(int? questionId, int answerId)>();

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

                        var questionId = answer.PkQuestionId;
                        userAnswers.Add((questionId, answerId));
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

                    var examJson = await res.Content.ReadAsStringAsync();
                    try
                    {
                        var createdExam = JsonConvert.DeserializeObject<Exam>(examJson);
                        var examId = createdExam?.ExamId;
                        foreach (var answerId in selectedAnswers)
                        {
                            using (HttpResponseMessage resAnswer = await _httpClient.GetAsync($"{urlAnswer}/{answerId}"))
                            {
                                if (resAnswer.IsSuccessStatusCode)
                                {
                                    var answerJson = await resAnswer.Content.ReadAsStringAsync();
                                    var answer = JsonConvert.DeserializeObject<Answer>(answerJson);
                                    var questionId = answer.PkQuestionId;

                                    await SaveUserAnswer(examId, questionId, answerId);
                                }
                            }
                        }
                        return View("Exam", createdExam);
                    }
                    catch (JsonReaderException ex)
                    {
                        return BadRequest(new { error = "Lỗi khi phân tích cú pháp JSON.", examJson, exceptionMessage = ex.Message });
                    }


                }
                else
                {
                    return BadRequest("Có lỗi khi lưu thông tin bài thi.");
                }
            }
        }

        private async Task SaveUserAnswer(int? examId, int? questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                PkExamId = examId,     
                PkQuestionId = questionId,
                PkAnswerId = answerId,  
                IsSelected = true   
            };

            using (HttpResponseMessage res = await _httpClient.PostAsJsonAsync("https://localhost:7272/api/UserAnswer", userAnswer))
            {
                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error saving user answer: {res.StatusCode} - {errorContent}");
                }
            }
        }



    }
}