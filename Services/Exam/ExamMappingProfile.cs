using AutoMapper;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;

namespace ExamApp.Services.Exam
{
    public class ExamMappingProfile : Profile
    {
        public ExamMappingProfile()
        {
            CreateMap<CreateExamRequestDto, Repositories.Exams.Exam>();
            CreateMap<UpdateExamRequestDto, Repositories.Exams.Exam>();
            CreateMap<Repositories.Exams.Exam, ExamWithQuestionsResponseDto>();
            CreateMap<Repositories.Exams.Exam, ExamWithInstructorResponseDto>();
            CreateMap<Repositories.Exams.Exam, ExamWithDetailsResponseDto>();
        }
    }
}
