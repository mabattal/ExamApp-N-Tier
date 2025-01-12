using ExamApp.Repositories.Enums;

namespace ExamApp.Repositories.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }

        // Navigation properties
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
        public virtual ICollection<Exam> CreatedExams { get; set; }
    }
}
