using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            ExamResults = new HashSet<ExamResult>();
        }

        public int QuestionId { get; set; }
        public int? PkCategoryId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string? QuestionImage { get; set; }

        public virtual Category? PkCategory { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
