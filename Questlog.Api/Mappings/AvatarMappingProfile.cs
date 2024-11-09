using AutoMapper;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class AvatarMappingProfile : Profile
{
    public AvatarMappingProfile()
    {
        CreateMap<Avatar, AvatarDto>().ReverseMap();
        CreateMap<UnlockedAvatar, AvatarDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Avatar.Name))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Avatar.DisplayName));
    }
}