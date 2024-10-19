using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_prn231_api.Models;

namespace project_prn231_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        project_prn231Context context = new project_prn231Context();

        // GET: api/category
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = context.Categories
                .Select(x => new
                {
                    x.CategoryId,
                    x.CategoryName
                })
                .ToList();

            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = context.Categories
                .Where(x => x.CategoryId == id)
                .Select(x => new
                {
                    x.CategoryId,
                    x.CategoryName
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound($"Danh mục với ID {id} không tồn tại.");
            }

            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public IActionResult Insert([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Thông tin danh mục không hợp lệ.");
            }

            context.Categories.Add(category);
            context.SaveChanges();

            return Ok("Thêm thành công");
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Thông tin danh mục không hợp lệ.");
            }

            var existingCategory = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (existingCategory == null)
            {
                return NotFound($"Danh mục với ID {id} không tồn tại.");
            }

            existingCategory.CategoryName = category.CategoryName;
            context.SaveChanges();

            return Ok(existingCategory);
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound($"Danh mục với ID {id} không tồn tại.");
            }

            // Kiểm tra các ràng buộc khóa ngoại trước khi xóa
            var relatedExams = context.Exams.Any(e => e.PkCategoryId == id);
            if (relatedExams)
            {
                return BadRequest("Không thể xóa danh mục vì nó đang được sử dụng trong các bài kiểm tra.");
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            return Ok("Xóa thành công");
        }
    }
}
