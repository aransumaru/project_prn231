using System;
using System.Collections.Generic;

namespace project_prn231.Models
{
    public partial class ExamResultHistory
    {
        public int ExamHistoryId { get; set; }
        public int? PkExamId { get; set; }
        public int? TotalCorrect { get; set; }
        public string? Result { get; set; }

        public virtual Exam? PkExam { get; set; }
    }
}
