namespace ExamApp.Services.Question
{
    public record QuestionResponseDto(
        int QuestionId,
        int ExamId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer
    );
}
