using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;
using System.Net.Http;

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
        private readonly string urlQuestion = "https://localhost:7272/api/Question"; // Địa chỉ API

        public async Task<IActionResult> Index() // Phương thức Index để hiển thị danh sách câu hỏi
        {
            using (HttpClient client = _httpClient)
            {
                // Gửi yêu cầu GET đến API
                using (HttpResponseMessage res = await client.GetAsync(urlQuestion))
                {
                    // Kiểm tra xem yêu cầu có thành công không
                    if (res.IsSuccessStatusCode)
                    {
                        using (HttpContent content = res.Content)
                        {
                            // Đọc nội dung trả về dưới dạng chuỗi
                            string result = await content.ReadAsStringAsync();

                            // Deserialize chuỗi JSON thành danh sách Question
                            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(result);

                            // Trả về view với danh sách câu hỏi
                            return View(questions);
                        }
                    }
                    else
                    {
                        // Xử lý trường hợp yêu cầu không thành công (có thể trả về một view lỗi)
                        return NotFound("Không tìm thấy danh sách câu hỏi.");
                    }
                }
            }
        }
    }
}
