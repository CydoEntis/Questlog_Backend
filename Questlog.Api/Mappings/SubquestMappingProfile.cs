using AutoMapper;
using Questlog.Application.Common.DTOs.Subquest.Request;
using Questlog.Application.Common.DTOs.Subquest.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class SubquestMappingProfile : Profile
{
    public SubquestMappingProfile()
    {
        CreateMap<Subquest, CreateSubquestRequestDto>().ReverseMap();
        CreateMap<Subquest, CreateSubquestResponseDto>().ReverseMap();
    }
}