using AutoMapper;
using Questlog.Application.Common.DTOs.Member.Request;
using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        CreateMap<Member, GetMemberResponseDto>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.User.CurrentLevel));

        CreateMap<Member, GetMemberAvatarResponseDto>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

        CreateMap<Member, CreateMemberRequestDto>().ReverseMap();
        CreateMap<Member, CreateMemberResponseDto>().ReverseMap();
    }
}