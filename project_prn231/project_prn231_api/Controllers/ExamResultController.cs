using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : Controller
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/examresult
        [HttpGet]
        public IActionResult GetAll()
        {
            var results = context.ExamResults
                .Select(x => new
                {
                    x.ExamResultId,
                    x.PkExamId,
                    x.PkQuestionId,
                    x.PkAnswerId
                })
                .ToList();

            return Ok(results);
        }

        // GET: api/examresult/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = context.ExamResults
                .Where(x => x.ExamResultId == id)
                .Select(x => new
                {
                    x.ExamResultId,
                    x.PkExamId,
                    x.PkQuestionId,
                    x.PkAnswerId
                })
                .FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/examresult
        [HttpPost]
        public IActionResult Insert(ExamResult examResult)
        {
            context.ExamResults.Add(examResult);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/examresult/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ExamResult examResult)
        {
            if (id != examResult.ExamResultId)
            {
                return BadRequest();
            }

            var existingResult = context.ExamResults.Find(id);
            if (existingResult == null)
            {
                return NotFound();
            }

            existingResult.PkExamId = examResult.PkExamId;
            existingResult.PkQuestionId = examResult.PkQuestionId;
            existingResult.PkAnswerId = examResult.PkAnswerId;

            context.SaveChanges();
            return Ok(existingResult);
        }

        // DELETE: api/examresult/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteExamResult(int id)
        {
            var result = context.ExamResults.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            context.ExamResults.Remove(result);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }
    }
}
