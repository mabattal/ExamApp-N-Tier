using AutoMapper;
using ExamApp.Services.User.Create;
using ExamApp.Services.User.Update;

namespace ExamApp.Services.User
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Repositories.Users.User, UserResponseDto>();
            CreateMap<CreateUserRequestDto, Repositories.Users.User>();
            CreateMap<UpdateUserRequestDto, Repositories.Users.User>();
        }
    }
}
