namespace ExamApp.Repositories.Entities
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; } // Dakika cinsinden
        public int CreatedBy { get; set; }

        // Navigation properties
        public virtual User Creator { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
