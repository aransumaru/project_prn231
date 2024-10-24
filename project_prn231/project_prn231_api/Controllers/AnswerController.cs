using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/answer
        [HttpGet]
        public IActionResult GetAll()
        {
            var answers = context.Answers
                .Select(a => new
                {
                    a.AnswerId,
                    a.PkQuestionId,
                    a.AnswerText,
                    a.AnswerImage,
                    a.IsCorrect,
                    a.PkUserId
                })
                .ToList();

            return Ok(answers);
        }

        // GET: api/answer/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var answer = context.Answers
                .Where(a => a.AnswerId == id)
                .Select(a => new
                {
                    a.AnswerId,
                    a.PkQuestionId,
                    a.AnswerText,
                    a.AnswerImage,
                    a.IsCorrect,
                    a.PkUserId
                })
                .FirstOrDefault();

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // POST: api/answer
        [HttpPost]
        public IActionResult Insert([FromBody] Answer answer)
        {
            if (answer == null)
            {
                return BadRequest("Thông tin câu trả lời không hợp lệ.");
            }

            context.Answers.Add(answer);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/answer/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Answer answer)
        {
            if (answer == null)
            {
                return BadRequest("Thông tin câu trả lời không hợp lệ.");
            }

            // Kiểm tra xem câu trả lời có tồn tại không
            var existingAnswer = context.Answers.FirstOrDefault(a => a.AnswerId == id);
            if (existingAnswer == null)
            {
                return NotFound($"Câu trả lời với ID {id} không tồn tại.");
            }

            // Cập nhật thông tin câu trả lời
            existingAnswer.AnswerText = answer.AnswerText;
            existingAnswer.AnswerImage = answer.AnswerImage;
            existingAnswer.IsCorrect = answer.IsCorrect;

            context.SaveChanges();

            return Ok(existingAnswer);
        }

        // DELETE: api/answer/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = context.Answers.Find(id);
            if (answer == null)
            {
                return NotFound($"Câu trả lời với ID {id} không tồn tại.");
            }


            context.Answers.Remove(answer);
            context.SaveChanges();

            return Ok("ZXóa thành công");
        }
        // GET: api/answer/GetByQuestionId?questionId={questionId}
        [HttpGet("GetByQuestionId")]
        public IActionResult GetByQuestionId(int questionId)
        {
            var answers = context.Answers
                .Where(a => a.PkQuestionId == questionId)
                .Select(a => new
                {
                    a.AnswerId,
                    a.PkQuestionId,
                    a.AnswerText,
                    a.AnswerImage,
                    a.IsCorrect,
                    a.PkUserId
                })
                .ToList();

            if (!answers.Any())
            {
                return NotFound($"Không có câu trả lời nào cho câu hỏi với ID {questionId}.");
            }

            return Ok(answers);
        }

    }
}
