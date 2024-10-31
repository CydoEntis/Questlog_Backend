using AutoMapper;
using Questlog.Application.Common.DTOs.Member;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.User.CurrentLevel));


        CreateMap<Member, CreateMemberDto>().ReverseMap();
        CreateMap<Member, UpdateMemberDto>().ReverseMap();
    }
}