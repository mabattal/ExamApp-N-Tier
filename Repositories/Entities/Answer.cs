namespace ExamApp.Repositories.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public string? SelectedAnswer { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
    }
}
