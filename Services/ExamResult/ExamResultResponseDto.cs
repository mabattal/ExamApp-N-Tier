namespace ExamApp.Services.ExamResult
{
    public record ExamResultResponseDto(
        int ResultId,
        int UserId,
        int ExamId,
        decimal? Score,
        DateTime StartDate,
        DateTime? CompletionDate,
        int? Duration,
        int TotalQuestions,
        int? CorrectAnswers,
        int? IncorrectAnswers
    );
}
