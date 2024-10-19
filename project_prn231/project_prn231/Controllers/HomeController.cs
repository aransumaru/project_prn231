using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_prn231.Models;
using System.Diagnostics;
using System.Net.Http;

namespace project_prn231.Controllers
{
    [Route("Home")]

    public class HomeController : Controller
    {
        private readonly string url = "https://localhost:7272/api/Category";

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

                            // Deserialize chuỗi JSON thành danh sách Category
                            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(result);

                            // Trả về view với danh sách danh mục
                            return View(categories);
                        }
                    }
                    else
                    {
                        // Xử lý trường hợp yêu cầu không thành công (có thể trả về một view lỗi)
                        return NotFound("Không tìm thấy danh mục.");
                    }
                }
            }
        }
    }
}
