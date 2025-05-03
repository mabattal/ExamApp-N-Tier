using AutoMapper;

namespace ExamApp.Services.ExamResult
{
    public class ExamResultMappingProfile : Profile
    {
        public ExamResultMappingProfile()
        {
            CreateMap<Repositories.ExamResults.ExamResult, ExamResultResponseDto>();

        }
    }
}
