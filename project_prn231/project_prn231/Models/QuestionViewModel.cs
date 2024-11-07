using static project_prn231.Controllers.HistoryController;

namespace project_prn231.Models
{
    public class QuestionViewModel
    {
        public string QuestionText { get; set; }
        public string QuestionImage { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }
}
