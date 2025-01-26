using ExamApp.Services.Question;

namespace ExamApp.Services.Exam
{
    public record ExamWithDetailsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration, // Dakika cinsinden
        int CreatedBy, // Eğitmen ID
        string InstructorMail, // Eğitmenin maili
        ICollection<QuestionResponseDto> Questions // Soruların detayları
    );
}