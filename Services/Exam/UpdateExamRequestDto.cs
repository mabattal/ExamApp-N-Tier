namespace ExamApp.Services.Exam
{
    public record UpdateExamRequestDto(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,        // Dakika cinsinden
        int CreatedBy
    );
}
