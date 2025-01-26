namespace ExamApp.Services.Question
{
    public record QuestionResponseDto(
        int QuestionId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer // A, B, C veya D
    );
}
