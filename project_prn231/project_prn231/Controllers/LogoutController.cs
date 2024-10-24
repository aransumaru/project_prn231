using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{
    [Route("Logout")]
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
