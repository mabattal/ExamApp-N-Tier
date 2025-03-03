namespace ExamApp.Services.Answer.Create
{
    public record CreateAnswerRequestDto(
        int UserId, 
        int ExamId, 
        int QuestionId, 
        string SelectedAnswer
        );
}
