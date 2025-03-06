using AutoMapper;
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
        }
    }
}
