using AutoMapper;
using ExamApp.Services.Question;
using ExamApp.Services.Question.Create;
using ExamApp.Services.Question.Update;
using ExamApp.Services.User;
using ExamApp.Services.User.Create;
using ExamApp.Services.User.Update;

namespace ExamApp.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User Mapping
            CreateMap<Repositories.Entities.User, UserResponseDto>();
            CreateMap<CreateUserRequestDto, Repositories.Entities.User>();
            CreateMap<UpdateUserRequestDto, Repositories.Entities.User>();

            //Question Mapping
            CreateMap<Repositories.Entities.Question, QuestionResponseDto>();
            CreateMap<CreateQuestionRequestDto, Repositories.Entities.Question>();
            CreateMap<UpdateQuestionRequestDto, Repositories.Entities.Question>();
            CreateMap<Repositories.Entities.Question, QuestionResponseWithoutCorrectAnswerDto>();
        }
    }
}
