using AutoMapper;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class AvatarMappingProfile : Profile
{
    public AvatarMappingProfile()
    {
        CreateMap<Avatar, AvatarDto>().ReverseMap();
    }
}