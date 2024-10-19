using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using project_prn231.Models; // Đảm bảo rằng namespace này chứa model Answer

namespace project_prn231.Controllers
{
    
    public class AnswerController : Controller
    {
        // Địa chỉ API của bạn
        private readonly string url = "https://localhost:7272/api/Answer";

        // Phương thức để lấy danh sách câu trả lời
        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                // Gửi yêu cầu GET đến API
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    // Kiểm tra xem yêu cầu có thành công không
                    if (res.IsSuccessStatusCode)
                    {
                        using (HttpContent content = res.Content)
                        {
                            // Đọc nội dung trả về dưới dạng chuỗi
                            string result = await content.ReadAsStringAsync();

                            // Deserialize chuỗi JSON thành danh sách Answer
                            List<Answer> answers = JsonConvert.DeserializeObject<List<Answer>>(result);

                            // Trả về view với danh sách câu trả lời
                            return View(answers);
                        }
                    }
                    else
                    {
                        // Xử lý trường hợp yêu cầu không thành công (có thể trả về một view lỗi)
                        return NotFound("Không tìm thấy câu trả lời.");
                    }
                }
            }
        }
    }
}
