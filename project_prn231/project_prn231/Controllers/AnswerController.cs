using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using project_prn231.Models;
using System.Text;

namespace project_prn231.Controllers
{
    [Route("Answer")]
    
    public class AnswerController : Controller
    {
        public AnswerController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlAnswer = "https://localhost:7272/api/Answer";

        [HttpGet("Create/{questionId}")]
        public IActionResult Create(int questionId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.QuestionId = questionId;
            return View();
        }

        [HttpPost("Create/{questionId}")]
        public async Task<IActionResult> Create(int questionId, Answer newAnswer)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            newAnswer.PkQuestionId = questionId;
            newAnswer.PkUserId = userId;

            var jsonContent = new StringContent(JsonConvert.SerializeObject(newAnswer), Encoding.UTF8, "application/json");
            using (HttpResponseMessage res = await _httpClient.PostAsync($"{urlAnswer}", jsonContent)) 
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detail", "Admin", new { id = questionId });
                }
                else
                {
                    return BadRequest("Tạo câu trả lời không thành công.");
                }
            }
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            var response = await _httpClient.GetAsync($"{urlAnswer}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var answerJson = await response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<Answer>(answerJson);
                return View(answer);
            }

            return NotFound($"Câu trả lời với ID {id} không tồn tại.");
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return BadRequest("ID không khớp.");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            using (HttpResponseMessage res = await _httpClient.PutAsJsonAsync($"{urlAnswer}/{id}", answer))
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detail", "Admin", new { id = answer.PkQuestionId });
                }
                else
                {
                    return BadRequest("Cập nhật không thành công.");
                }
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            using (HttpResponseMessage res = await _httpClient.GetAsync($"{urlAnswer}/{id}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();
                    var answer = JsonConvert.DeserializeObject<Answer>(result);

                    return View(answer);
                }
                else
                {
                    return NotFound($"Câu hỏi với ID {id} không tồn tại.");
                }
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            var getResponse = await _httpClient.GetAsync($"{urlAnswer}/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                return BadRequest("Không thể tìm thấy answer để xóa.");

            }
            var answer = await getResponse.Content.ReadAsAsync<Answer>();
            using (HttpResponseMessage res = await _httpClient.DeleteAsync($"{urlAnswer}/{id}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detail", "Admin", new { id = answer.PkQuestionId });
                }
                else
                {
                    return BadRequest("Xóa không thành công.");
                }
            }
        }
    }
}
