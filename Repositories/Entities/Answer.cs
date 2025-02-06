namespace ExamApp.Repositories.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty; // A, B, C veya D
        public DateTime CreatedDate { get; set; }
        public bool IsCorrect { get; set; }
    }
}
