namespace ExamApp.Services.Answer
{
    public record CreateAnswerRequestDto(int UserId, int QuestionId, string SelectedAnswer, bool IsCorrect);
}
