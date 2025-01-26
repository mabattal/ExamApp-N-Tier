namespace ExamApp.Services.Exam
{
    public record ActiveExamsResponseDto(
        int ExamId,
        string Title,
        DateTime StartDate,
        DateTime EndDate
    );
}
