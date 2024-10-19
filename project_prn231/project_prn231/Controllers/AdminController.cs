using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
