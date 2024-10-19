using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultHistoryController : Controller
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/examresulthistory
        [HttpGet]
        public IActionResult GetAll()
        {
            var histories = context.ExamResultHistorys
                .Select(x => new
                {
                    x.ExamHistoryId,
                    x.PkExamId,
                    x.Result,
                    x.TotalCorrect
                })
                .ToList();

            return Ok(histories);
        }

        // GET: api/examresulthistory/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var history = context.ExamResultHistorys
                .Where(x => x.ExamHistoryId == id)
                .Select(x => new
                {
                    x.ExamHistoryId,
                    x.PkExamId,
                    x.Result,
                    x.TotalCorrect
                })
                .FirstOrDefault();

            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        // POST: api/examresulthistory
        [HttpPost]
        public IActionResult Insert(ExamResultHistory examResultHistory)
        {
            context.ExamResultHistorys.Add(examResultHistory);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/examresulthistory/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ExamResultHistory examResultHistory)
        {
            if (id != examResultHistory.ExamHistoryId)
            {
                return BadRequest();
            }

            var existingHistory = context.ExamResultHistorys.Find(id);
            if (existingHistory == null)
            {
                return NotFound();
            }

            existingHistory.PkExamId = examResultHistory.PkExamId;
            existingHistory.Result = examResultHistory.Result;
            existingHistory.TotalCorrect = examResultHistory.TotalCorrect;

            context.SaveChanges();
            return Ok(existingHistory);
        }

        // DELETE: api/examresulthistory/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteExamResultHistory(int id)
        {
            var history = context.ExamResultHistorys.Find(id);
            if (history == null)
            {
                return NotFound();
            }

            context.ExamResultHistorys.Remove(history);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }
    }
}
