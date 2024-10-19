using System;
using System.Collections.Generic;

namespace project_prn231.Models
{
    public partial class Answer
    {
        public Answer()
        {
            ExamResults = new HashSet<ExamResult>();
        }

        public int AnswerId { get; set; }
        public int? PkQuestionId { get; set; }
        public string AnswerText { get; set; } = null!;
        public string? AnswerImage { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Question? PkQuestion { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
