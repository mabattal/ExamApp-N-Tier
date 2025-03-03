namespace ExamApp.Services.Question.Update
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
