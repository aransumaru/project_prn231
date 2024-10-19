using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class ExamResult
    {
        public int ExamResultId { get; set; }
        public int? PkExamId { get; set; }
        public int? PkQuestionId { get; set; }
        public int? PkAnswerId { get; set; }

        public virtual Answer? PkAnswer { get; set; }
        public virtual Exam? PkExam { get; set; }
        public virtual Question? PkQuestion { get; set; }
    }
}
