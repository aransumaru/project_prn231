using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class Exam
    {
        public int ExamId { get; set; }
        public int? PkUserId { get; set; }
        public int? PkCategoryId { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? Point { get; set; }

        public virtual Category? PkCategory { get; set; }
        public virtual User? PkUser { get; set; }
    }
}
