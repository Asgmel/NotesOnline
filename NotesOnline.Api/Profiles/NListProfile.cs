using AutoMapper;
using NotesOnline.Dtos;
using NotesOnline.Api.Models;

namespace NotesOnline.Api.Profiles
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NListProfile : Profile
    {
        public NListProfile()
        {
            CreateMap<SList, SListReadDto>();
            CreateMap<SListCreateDto, SList>();
            CreateMap<SList, SListUpdateDto>();
            CreateMap<SListUpdateDto, SList>();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
