namespace ExamApp.Repositories.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;

        // Navigation properties
        public virtual Exam Exam { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
