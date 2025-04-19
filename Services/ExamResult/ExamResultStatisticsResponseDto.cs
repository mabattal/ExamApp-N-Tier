namespace ExamApp.Services.ExamResult
{
    public record ExamResultStatisticsResponseDto(
        int StudentCount,
        decimal AverageScore,
        decimal MaxScore,
        decimal MinScore
        );
}
