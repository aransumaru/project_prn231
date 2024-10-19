﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : Controller
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/question
        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = context.Questions
                .Select(x => new
                {
                    x.QuestionId,
                    x.PkCategoryId,
                    x.QuestionText,
                    x.QuestionImage
                })
                .ToList();

            return Ok(questions);
        }

        // GET: api/question/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var question = context.Questions
                .Where(x => x.QuestionId == id)
                .Select(x => new
                {
                    x.QuestionId,
                    x.PkCategoryId,
                    x.QuestionText,
                    x.QuestionImage
                })
                .FirstOrDefault();

            if (question == null)
            {
                return NotFound($"Câu hỏi với ID {id} không tồn tại.");
            }

            return Ok(question);
        }

        // POST: api/question
        [HttpPost]
        public IActionResult Insert([FromBody] Question question)
        {
            if (question == null)
            {
                return BadRequest("Thông tin câu hỏi không hợp lệ.");
            }

            // Validate fields
            if (string.IsNullOrWhiteSpace(question.QuestionText))
            {
                return BadRequest("Nội dung câu hỏi là bắt buộc.");
            }

            if (question.PkCategoryId <= 0)
            {
                return BadRequest("ID danh mục không hợp lệ.");
            }

            context.Questions.Add(question);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/question/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Question question)
        {
            if (question == null)
            {
                return BadRequest("Thông tin câu hỏi không hợp lệ.");
            }

            if (id != question.QuestionId)
            {
                return BadRequest("ID không khớp.");
            }

            var existingQuestion = context.Questions.Find(id);
            if (existingQuestion == null)
            {
                return NotFound($"Câu hỏi với ID {id} không tồn tại.");
            }

            // Validate fields
            if (string.IsNullOrWhiteSpace(question.QuestionText))
            {
                return BadRequest("Nội dung câu hỏi là bắt buộc.");
            }

            if (question.PkCategoryId <= 0)
            {
                return BadRequest("ID danh mục không hợp lệ.");
            }

            existingQuestion.PkCategoryId = question.PkCategoryId;
            existingQuestion.QuestionText = question.QuestionText;
            existingQuestion.QuestionImage = question.QuestionImage;

            context.SaveChanges();
            return Ok(existingQuestion);
        }

        // DELETE: api/question/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            // Tìm câu hỏi
            var question = context.Questions.Include(q => q.Answers).FirstOrDefault(q => q.QuestionId == id);

            // Kiểm tra xem câu hỏi có tồn tại không
            if (question == null)
            {
                return NotFound($"Câu hỏi với ID {id} không tồn tại.");
            }

            // Kiểm tra xem có khóa ngoại nào liên quan đến câu hỏi không
            var hasOtherForeignKeys = context.ExamResults.Any(er => er.PkQuestionId == id); // Kiểm tra bảng ExamResults

            if (hasOtherForeignKeys)
            {
                return BadRequest("Không thể xóa câu hỏi này vì nó đang được sử dụng trong kết quả thi.");
            }

            // Nếu câu hỏi có các answer liên quan, xóa tất cả answer đó
            if (question.Answers.Any())
            {
                context.Answers.RemoveRange(question.Answers); // Xóa tất cả các answer
            }

            // Xóa câu hỏi
            context.Questions.Remove(question);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }


    }
}