namespace ExamApp.Services.Exam.Update
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
