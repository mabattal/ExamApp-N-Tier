using AutoMapper;
using ExamApp.Services.Question.Create;
using ExamApp.Services.Question.Update;

namespace ExamApp.Services.Question
{
    public class QuestionMappingProfile:Profile
    {
        public QuestionMappingProfile()
        {
            CreateMap<Repositories.Questions.Question, QuestionResponseDto>();
            CreateMap<CreateQuestionRequestDto, Repositories.Questions.Question>();
            CreateMap<UpdateQuestionRequestDto, Repositories.Questions.Question>();
            CreateMap<Repositories.Questions.Question, QuestionResponseWithoutCorrectAnswerDto>();
        }
    }
}
