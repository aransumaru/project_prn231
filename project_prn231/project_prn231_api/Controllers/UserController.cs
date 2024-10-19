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
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }
            // Kiểm tra xem email đã tồn tại chưa
            if (context.Users.Any(u => u.Email == user.Email))
            {
                return Conflict("Email đã được sử dụng.");
            }
            context.Users.Add(user);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.IsAdmin
            });
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

            if (context.ExamResults.Any(er => er.PkExamId != null && context.Exams.Any(e => e.ExamId == er.PkExamId && e.PkUserId == id)))
            {
                return BadRequest("Không thể xóa người dùng vì họ có kết quả thi liên kết.");
            }

            if (context.ExamResultHistorys.Any(eh => eh.PkExamId != null && context.Exams.Any(e => e.ExamId == eh.PkExamId && e.PkUserId == id)))
            {
                return BadRequest("Không thể xóa người dùng vì họ có lịch sử thi liên kết.");
            }

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }
    }

}

