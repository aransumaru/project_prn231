namespace project_prn231.Models
{
    public partial class UserAnswer
    {
        public int UserAnswerId { get; set; }
        public int? PkExamId { get; set; }
        public int? PkQuestionId { get; set; }
        public int? PkAnswerId { get; set; }
        public bool? IsSelected { get; set; }

        public virtual Answer? PkAnswer { get; set; }
        public virtual Exam? PkExam { get; set; }
        public virtual Question? PkQuestion { get; set; }
    }
}
