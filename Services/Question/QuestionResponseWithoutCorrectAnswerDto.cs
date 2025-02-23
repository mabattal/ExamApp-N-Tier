namespace ExamApp.Services.Question
{
    public record QuestionResponseWithoutCorrectAnswerDto(
        int QuestionId,
        int ExamId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD
    );
}
