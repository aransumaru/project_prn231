using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class User
    {
        public User()
        {
            Answers = new HashSet<Answer>();
            Exams = new HashSet<Exam>();
            Questions = new HashSet<Question>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? IsAdmin { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
