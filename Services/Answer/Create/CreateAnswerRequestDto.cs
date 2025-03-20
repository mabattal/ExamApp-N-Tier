namespace ExamApp.Services.Answer.Create
{
    public record CreateAnswerRequestDto(
        int ExamId, 
        int QuestionId, 
        string SelectedAnswer
        );
}
