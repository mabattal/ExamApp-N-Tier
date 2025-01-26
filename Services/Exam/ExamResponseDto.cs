namespace ExamApp.Services.Exam
{
    public record ExamResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration, // Dakika cinsinden
        int CreatedBy // Eğitmen ID
        //string InstructorName, // Eğitmenin adı
        //ICollection<int> QuestionIds // Soruların ID'leri
    );
}

