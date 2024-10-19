using System;
using System.Collections.Generic;

namespace project_prn231.Models
{
    public partial class Category
    {
        public Category()
        {
            Exams = new HashSet<Exam>();
            Questions = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
