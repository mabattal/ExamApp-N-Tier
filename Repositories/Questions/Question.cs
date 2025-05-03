using ExamApp.Repositories.Exams;

namespace ExamApp.Repositories.Questions
{
    public class Question : IAuditEntity
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
