using Microsoft.AspNetCore.Mvc;
using project_prn231_api.Models; // Đảm bảo đường dẫn chính xác đến mô hình
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswerController : ControllerBase
    {
        project_prn231Context context = new project_prn231Context();


        // GET: api/UserAnswer
        [HttpGet]
        public IActionResult GetAll()
        {
            var userAnswer = context.UserAnswers
                .Select(ua => new
                {
                    ua.UserAnswerId,
                    ua.PkExamId,
                    ua.PkQuestionId,
                    ua.PkAnswerId,
                    ua.IsSelected,

                })
                .ToList();

            return Ok(userAnswer);
        }

        // GET: api/UserAnswer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAnswer>> GetUserAnswer(int id)
        {
            var userAnswer = await context.UserAnswers.FindAsync(id);

            if (userAnswer == null)
            {
                return NotFound();
            }

            return userAnswer;
        }

        // POST: api/UserAnswer
        [HttpPost]
        public async Task<IActionResult> PostUserAnswer([FromBody] UserAnswer userAnswer)
        {
            if (userAnswer == null)
            {
                return BadRequest("Invalid user answer.");
            }

            context.UserAnswers.Add(userAnswer);
            await context.SaveChangesAsync();

            return Ok(userAnswer);
        }
        // PUT: api/UserAnswer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAnswer(int id, UserAnswer userAnswer)
        {
            if (id != userAnswer.UserAnswerId)
            {
                return BadRequest();
            }

            context.Entry(userAnswer).State = EntityState.Modified;
            await context.SaveChangesAsync();


            return NoContent();
        }

        // DELETE: api/UserAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAnswer(int id)
        {
            var userAnswer = await context.UserAnswers.FindAsync(id);
            if (userAnswer == null)
            {
                return NotFound();
            }

            context.UserAnswers.Remove(userAnswer);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/UserAnswer/Exam/{examId}
        [HttpGet("Exam/{examId}")]
        public async Task<IActionResult> GetUserAnswersByExamId(int examId)
        {
            var userAnswers = await context.UserAnswers
                .Where(ua => ua.PkExamId == examId)
                .Include(ua => ua.PkAnswer) // Kết nối với bảng Answer
                .ToListAsync();

            if (userAnswers == null || !userAnswers.Any())
            {
                return NotFound("Không có câu trả lời nào cho bài thi này.");
            }

            // Chỉ định thông tin cần thiết
            var userAnswerDetails = userAnswers.Select(ua => new
            {
                ua.PkAnswerId,
                ua.IsSelected,
                AnswerText = ua.PkAnswer.AnswerText,
                IsCorrect = ua.PkAnswer.IsCorrect
            }).ToList();

            return Ok(userAnswerDetails);
        }

    }
}
