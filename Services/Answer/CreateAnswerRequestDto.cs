namespace ExamApp.Services.Answer
{
    public record CreateAnswerRequestDto(int UserId, int ExamId, int QuestionId, string SelectedAnswer, bool IsCorrect);
}
