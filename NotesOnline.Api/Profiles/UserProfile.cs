using AutoMapper;
using NotesOnline.Dtos;
using NotesOnline.Api.Models;

namespace NotesOnline.Api.Profiles
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserAuthDto, UserReadDto>();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
