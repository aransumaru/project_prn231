using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/exam
        [HttpGet]
        public IActionResult GetAll()
        {
            var exams = context.Exams
                .Select(x => new
                {
                    x.ExamId,
                    x.PkUserId,
                    x.PkCategoryId,
                    x.ExamDate,
                    x.Point,
                    CategoryName = x.PkCategory.CategoryName // Nếu cần hiển thị tên danh mục
                })
                .ToList();

            return Ok(exams);
        }

        // GET: api/exam/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var exam = context.Exams
                .Where(x => x.ExamId == id)
                .Select(x => new
                {
                    x.ExamId,
                    x.PkUserId,
                    x.PkCategoryId,
                    x.ExamDate,
                    x.Point,
                    CategoryName = x.PkCategory.CategoryName // Nếu cần hiển thị tên danh mục
                })
                .FirstOrDefault();

            if (exam == null)
            {
                return NotFound($"Bài kiểm tra với ID {id} không tồn tại.");
            }

            return Ok(exam);
        }

        // POST: api/exam
        [HttpPost]
        public IActionResult Insert([FromBody] Exam exam)
        {
            if (exam == null)
            {
                return BadRequest("Thông tin bài kiểm tra không hợp lệ.");
            }

            context.Exams.Add(exam);
            context.SaveChanges();

            return Ok(exam);
        }

        // PUT: api/exam/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Exam exam)
        {
            if (exam == null)
            {
                return BadRequest("Thông tin bài kiểm tra không hợp lệ.");
            }

            var existingExam = context.Exams.FirstOrDefault(e => e.ExamId == id);
            if (existingExam == null)
            {
                return NotFound($"Bài kiểm tra với ID {id} không tồn tại.");
            }

            existingExam.PkUserId = exam.PkUserId;
            existingExam.PkCategoryId = exam.PkCategoryId;
            existingExam.ExamDate = exam.ExamDate;
            existingExam.Point = exam.Point;
            context.SaveChanges();

            return Ok(existingExam);
        }

        // DELETE: api/exam/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exam = context.Exams.Find(id);
            if (exam == null)
            {
                return NotFound($"Bài kiểm tra với ID {id} không tồn tại.");
            }


            context.Exams.Remove(exam);
            context.SaveChanges();

            return NoContent();
        }

        // GET: api/exam/user/{userId}
        [HttpGet("User/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var exams = context.Exams
                .Include(x => x.PkUser) // Lấy thông tin User
                .Include(x => x.PkCategory) // Lấy thông tin Category
                .Where(x => x.PkUserId == userId)
                .Select(x => new
                {
                    x.ExamId,
                    x.PkUserId,
                    Username = x.PkUser.Username, // Lấy Username từ PkUser
                    x.PkCategoryId,
                    CategoryName = x.PkCategory.CategoryName, // Lấy CategoryName từ PkCategory
                    x.ExamDate,
                    x.Point
                })
                .ToList();

            if (!exams.Any())
            {
                return NotFound($"Không tìm thấy bài thi nào cho người dùng với ID {userId}.");
            }

            return Ok(exams);
        }

    }
}
