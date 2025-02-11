namespace ExamApp.Services.Exam
{
    public record ExamResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,
        int CreatedBy
    );
}

