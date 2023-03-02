using AutoMapper;
using Search.Users.Dtos;
using Users.Dtos;
using Users.Models;

namespace Users.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            //Source -> Target
            CreateMap<CreateBetDto, User>();
            CreateMap<User, CreateBetDto>();
            CreateMap<UserReadDto, User>();
            CreateMap<User, UserReadDto>();
            CreateMap<UserProfileDto, User>();
            CreateMap<User, UserProfileDto>();
            CreateMap<User, SearchReadUsersDto>();
            CreateMap<SearchReadUsersDto,User>();
        }
    }
}