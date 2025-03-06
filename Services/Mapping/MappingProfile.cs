﻿using AutoMapper;
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
            CreateMap<Repositories.Entities.User, UserResponseDto>();
            CreateMap<CreateUserRequestDto, Repositories.Entities.User>();
            CreateMap<UpdateUserRequestDto, Repositories.Entities.User>();

            //Question Mapping
            CreateMap<Repositories.Entities.Question, QuestionResponseDto>();
            CreateMap<CreateQuestionRequestDto, Repositories.Entities.Question>();
            CreateMap<UpdateQuestionRequestDto, Repositories.Entities.Question>();
            CreateMap<Repositories.Entities.Question, QuestionResponseWithoutCorrectAnswerDto>();

            //ExamResult Mapping
            CreateMap<Repositories.Entities.ExamResult, ExamResultResponseDto>();

            //Exam Mapping
            CreateMap<CreateExamRequestDto, Repositories.Entities.Exam>();
            CreateMap<UpdateExamRequestDto, Repositories.Entities.Exam>();
            CreateMap<Repositories.Entities.Exam, ExamWithQuestionsResponseDto>();
            CreateMap<Repositories.Entities.Exam, ExamWithInstructorResponseDto>();
            CreateMap<Repositories.Entities.Exam, ExamWithDetailsResponseDto>();

            //Answer Mapping
            CreateMap<Repositories.Entities.Answer, AnswerResponseDto>();
            CreateMap<CreateAnswerRequestDto, Repositories.Entities.Answer>();
            CreateMap<UpdateAnswerRequestDto, Repositories.Entities.Answer>();
        }
    }
}
