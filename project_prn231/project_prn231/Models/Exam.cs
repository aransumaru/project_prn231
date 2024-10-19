using System;
using System.Collections.Generic;

namespace project_prn231.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamResultHistories = new HashSet<ExamResultHistory>();
            ExamResults = new HashSet<ExamResult>();
        }

        public int ExamId { get; set; }
        public int? PkUserId { get; set; }
        public int? PkCategoryId { get; set; }
        public DateTime? ExamDate { get; set; }

        public virtual Category? PkCategory { get; set; }
        public virtual User? PkUser { get; set; }
        public virtual ICollection<ExamResultHistory> ExamResultHistories { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
