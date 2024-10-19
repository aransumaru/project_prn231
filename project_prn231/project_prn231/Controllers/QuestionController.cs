using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{

    [Route("Question")]
    public class QuestionController : Controller
    {
        public QuestionController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        private readonly HttpClient _httpClient;
        private readonly string urlQuestion = "https://localhost:7272/api/Question";
        public IActionResult Index()
        {
            return View();
        }
    }
}
