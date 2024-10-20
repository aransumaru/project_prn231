using Microsoft.AspNetCore.Mvc;

namespace project_prn231.Controllers
{
    [Route("Logout")]
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            // Xóa thông tin session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập sau khi đăng xuất
            return RedirectToAction("Login", "Login");
        }
    }
}
