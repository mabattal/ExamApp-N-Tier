namespace ExamApp.Services.Answer.Update
{
    public record UpdateAnswerRequestDto(
        string SelectedAnswer);

    //public record AnswerResponseDto
    //{
    //    public int AnswerId { get; init; }
    //    public int UserId { get; init; }
    //    public int QuestionId { get; init; }
    //    public string SelectedAnswer { get; init; } = string.Empty;
    //    public bool IsCorrect { get; init; }
    //}
}
