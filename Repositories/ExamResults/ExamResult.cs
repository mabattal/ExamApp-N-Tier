using ExamApp.Repositories.Exams;
using ExamApp.Repositories.Users;

namespace ExamApp.Repositories.ExamResults
{
    public class ExamResult
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? CompletionDate { get; set; }
        public int? Duration { get; set; }
        public int TotalQuestions { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? IncorrectAnswers { get; set; }
        public int? EmptyAnswers { get; set; }
        public decimal? Score { get; set; }
    }
}
