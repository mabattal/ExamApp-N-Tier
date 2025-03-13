using AutoMapper;
using ExamApp.Services.Answer;
using ExamApp.Services.Answer.Create;
using ExamApp.Services.Answer.Update;
using ExamApp.Services.Exam;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;
using ExamApp.Services.ExamResult;
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
            CreateMap<Repositories.Users.User, UserResponseDto>();
            CreateMap<CreateUserRequestDto, Repositories.Users.User>();
            CreateMap<UpdateUserRequestDto, Repositories.Users.User>();

            //Question Mapping
            CreateMap<Repositories.Questions.Question, QuestionResponseDto>();
            CreateMap<CreateQuestionRequestDto, Repositories.Questions.Question>();
            CreateMap<UpdateQuestionRequestDto, Repositories.Questions.Question>();
            CreateMap<Repositories.Questions.Question, QuestionResponseWithoutCorrectAnswerDto>();

            //ExamResult Mapping
            CreateMap<Repositories.ExamResults.ExamResult, ExamResultResponseDto>();

            //Exam Mapping
            CreateMap<CreateExamRequestDto, Repositories.Exams.Exam>();
            CreateMap<UpdateExamRequestDto, Repositories.Exams.Exam>();
            CreateMap<Repositories.Exams.Exam, ExamWithQuestionsResponseDto>();
            CreateMap<Repositories.Exams.Exam, ExamWithInstructorResponseDto>();
            CreateMap<Repositories.Exams.Exam, ExamWithDetailsResponseDto>();

            //Answer Mapping
            CreateMap<Repositories.Answers.Answer, AnswerResponseDto>();
            CreateMap<CreateAnswerRequestDto, Repositories.Answers.Answer>();
            CreateMap<UpdateAnswerRequestDto, Repositories.Answers.Answer>();
        }
    }
}
