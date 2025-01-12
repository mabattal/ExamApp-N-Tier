namespace ExamApp.Repositories.Entities
{
    public class ExamResult
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public decimal Score { get; set; }
        public DateTime CompletionDate { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
