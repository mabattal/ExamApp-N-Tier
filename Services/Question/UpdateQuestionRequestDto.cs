namespace ExamApp.Services.Question
{
    public record UpdateQuestionRequestDto(
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer
        );
}
