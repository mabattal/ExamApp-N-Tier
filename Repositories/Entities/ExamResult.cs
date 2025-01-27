namespace ExamApp.Repositories.Entities
{
    public class ExamResult
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public double? Score { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? Duration { get; set; }
        public int TotalQuestions { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? IncorrectAnswers { get; set; }
    }
}
