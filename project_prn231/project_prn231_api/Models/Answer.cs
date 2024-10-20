using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int? PkQuestionId { get; set; }
        public string AnswerText { get; set; } = null!;
        public string? AnswerImage { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Question? PkQuestion { get; set; }
    }
}
