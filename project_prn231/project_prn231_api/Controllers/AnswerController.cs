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
                .Select(x => new
                {
                    x.AnswerId,
                    x.PkQuestionId,
                    x.AnswerText,
                    x.AnswerImage,
                    x.IsCorrect
                })
                .ToList();

            return Ok(answers);
        }

        // GET: api/answer/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var answer = context.Answers
                .Where(x => x.AnswerId == id)
                .Select(x => new
                {
                    x.AnswerId,
                    x.PkQuestionId,
                    x.AnswerText,
                    x.AnswerImage,
                    x.IsCorrect
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
            existingAnswer.PkQuestionId = answer.PkQuestionId;
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

            // Kiểm tra các ràng buộc khóa ngoại trước khi xóa
            var relatedExamResults = context.ExamResults.Any(er => er.PkAnswerId == id);
            if (relatedExamResults)
            {
                return BadRequest("Không thể xóa câu trả lời vì nó đang được sử dụng trong các kết quả kiểm tra.");
            }

            context.Answers.Remove(answer);
            context.SaveChanges();

            return Ok("ZXóa thành công");
        }
    }
}
