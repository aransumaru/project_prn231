using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{
    [Route("Logout")]
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
