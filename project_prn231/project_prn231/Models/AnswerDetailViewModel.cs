﻿namespace project_prn231.Models
{
    public class AnswerDetailViewModel
    {
        public string QuestionText { get; set; }
        public string QuestionImage { get; set; }
        public string AnswerText { get; set; }
        public string AnswerImage { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }
}
