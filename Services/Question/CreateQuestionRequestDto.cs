namespace ExamApp.Services.Question
{
    public record CreateQuestionRequestDto(
        int ExamId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer
        );
}
