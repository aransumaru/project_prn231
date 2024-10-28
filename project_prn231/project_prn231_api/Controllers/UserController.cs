using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/user
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = context.Users
                .Select(x => new
                {
                    x.UserId,
                    x.Username,
                    x.Email,
                    x.IsAdmin
                })
                .ToList();

            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = context.Users
                .Where(x => x.UserId == id)
                .Select(x => new
                {
                    x.UserId,
                    x.Username,
                    x.Email,
                    x.IsAdmin
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Insert([FromBody] User user)
        {

            if (context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("Email đã được sử dụng.");
            }
            var existingUser = context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest("Người dùng đã tồn tại.");
            }
            context.Users.Add(user);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            // Kiểm tra tính hợp lệ
            if (!ModelState.IsValid)
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            // Tìm user theo ID
            var existingUser = context.Users.FirstOrDefault(u => u.UserId == id);
            if (existingUser != null)
            {
                // Cập nhật user
                existingUser.Username = user.Username;
                existingUser.Email = user.Username;
                existingUser.Password = user.Password;
                existingUser.IsAdmin = user.IsAdmin;

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Trả về thông tin user đã cập nhật
                return Ok(existingUser);
            }

            // Nếu không tìm thấy user với ID cho trước
            return NotFound($"Người dùng với ID {id} không tồn tại.");
        }



        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            // Kiểm tra xem người dùng có tồn tại không
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Kiểm tra các liên kết đến người dùng
            if (context.Exams.Any(e => e.PkUserId == id))
            {
                return BadRequest("Không thể xóa người dùng vì họ có bài thi liên kết.");
            }


            context.Users.Remove(user);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }
        // POST: api/user/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Thông tin đăng nhập không hợp lệ.");
            }

            var user = context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Thông tin đăng nhập không đúng.");
            }

            return Ok(new
            {
                user.UserId,
                user.IsAdmin
            });
        }

    }

}

