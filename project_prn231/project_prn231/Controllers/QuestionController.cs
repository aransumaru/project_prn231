using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{

    [Route("Question")]
    public class QuestionController : Controller
    {
        private readonly string urlQuestion = "https://localhost:7272/api/Question";
        public IActionResult Index()
        {
            return View();
        }
    }
}
