using AutoMapper;
using ExamApp.Services.Answer.Create;
using ExamApp.Services.Answer.Update;

namespace ExamApp.Services.Answer
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile()
        {
            CreateMap<Repositories.Answers.Answer, AnswerResponseDto>();
            CreateMap<CreateAnswerRequestDto, Repositories.Answers.Answer>();
            CreateMap<UpdateAnswerRequestDto, Repositories.Answers.Answer>();
        }
    }
}
