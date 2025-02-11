namespace ExamApp.Services.Exam
{
    public record CreateExamRequestDto(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,
        int CreatedBy
    );
}
