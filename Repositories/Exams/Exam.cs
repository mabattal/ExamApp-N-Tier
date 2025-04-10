using ExamApp.Repositories.Questions;
using ExamApp.Repositories.Users;

namespace ExamApp.Repositories.Exams
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int Duration { get; set; } // Dakika cinsinden
        public int CreatedBy { get; set; } // Eğitmen ID
        public User Instructor { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public bool IsDeleted { get; set; } = false;
    }
}
