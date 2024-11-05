using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;
using System.Net.Http;
using System.Text;

namespace project_prn231.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlQuestion = "https://localhost:7272/api/Question";
        private readonly string urlCategory = "https://localhost:7272/api/Category";

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            using (HttpClient client = _httpClient)
            {
                string url = $"{urlQuestion}/GetByUser?userId={userId.Value}";
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        using (HttpContent content = res.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(result);
                            return View(questions);
                        }
                    }
                    else
                    {
                        return View(new List<Question>());
                    }
                }
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Question question)
        {
            if (question == null)
            {
                return BadRequest("Thông tin câu hỏi không hợp lệ.");
            }
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Login");
                }

                // Gán Pk_UserId vào câu hỏi
                question.PkUserId = userId.Value;
                var json = JsonConvert.SerializeObject(question);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                using (HttpResponseMessage res = await _httpClient.PostAsync(urlQuestion, content))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    string questionDetails = JsonConvert.SerializeObject(question, Formatting.Indented);
                    return BadRequest("Có lỗi xảy ra khi thêm câu hỏi: " + res.ReasonPhrase + "\nThông tin câu hỏi: " + questionDetails);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi: " + ex.Message);
            }

        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            using (HttpResponseMessage res = await _httpClient.GetAsync(urlCategory))
            {
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<Category>>(result);
                    ViewBag.Categories = categories;
                }
                else
                {
                    ViewBag.Categories = new List<Category>();
                }
            }
            return View();
        }
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            using (HttpResponseMessage res = await _httpClient.GetAsync($"{urlQuestion}/{id}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();
                    var question = JsonConvert.DeserializeObject<Question>(result);

                    using (HttpResponseMessage categoryRes = await _httpClient.GetAsync(urlCategory))
                    {
                        if (categoryRes.IsSuccessStatusCode)
                        {
                            string categoryResult = await categoryRes.Content.ReadAsStringAsync();
                            var categories = JsonConvert.DeserializeObject<List<Category>>(categoryResult);
                            ViewBag.Categories = categories;
                        }
                    }

                    return View(question);
                }
                else
                {
                    return NotFound($"Câu hỏi với ID {id} không tồn tại.");
                }
            }
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest("ID không khớp.");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            // Gán Pk_UserId vào câu hỏi
            question.PkUserId = userId.Value;
            using (HttpResponseMessage res = await _httpClient.PutAsJsonAsync($"{urlQuestion}/{id}", question))
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Cập nhật không thành công.");
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
            using (HttpResponseMessage res = await _httpClient.DeleteAsync($"{urlQuestion}/{id}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Xóa không thành công.");
                }
            }
        }
        // GET: Admin/Detail/{id}
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            using (HttpResponseMessage res = await _httpClient.GetAsync($"{urlQuestion}/{id}"))
            {
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();

                    // Deserialize dữ liệu thành object Question chứa cả Answers
                    var questionDetail = JsonConvert.DeserializeObject<Question>(result);

                    return View(questionDetail); // Trả về view với thông tin chi tiết
                }
                else
                {
                    return NotFound("Không tìm thấy thông tin câu hỏi.");
                }
            }
        }

        [HttpPost("DeleteSelected")]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> questionIds)
        {
            if (questionIds == null || !questionIds.Any())
            {
                return BadRequest("Không có câu hỏi nào được chọn.");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            foreach (var id in questionIds)
            {
                using (HttpResponseMessage res = await _httpClient.DeleteAsync($"{urlQuestion}/{id}"))
                {
                    if (!res.IsSuccessStatusCode)
                    {
                        return BadRequest($"Xóa câu hỏi với ID {id} không thành công.");
                    }
                }
            }

            return Ok("Xóa các câu hỏi thành công.");
        }


    }
}
