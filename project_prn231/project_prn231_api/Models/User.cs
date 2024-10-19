using System;
using System.Collections.Generic;

namespace project_prn231_api.Models
{
    public partial class User
    {
        public User()
        {
            Exams = new HashSet<Exam>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? IsAdmin { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
