namespace ExamApp.Services.ExamResult
{
    public record ExamResultResponseDto(
        int ResultId,
        int UserId,
        int ExamId,
        decimal? Score,
        DateTimeOffset StartDate,
        DateTimeOffset? CompletionDate,
        int? Duration,
        int TotalQuestions,
        int? CorrectAnswers,
        int? IncorrectAnswers,
        int? EmptyAnswers
    );
}
