using AutoMapper;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class GuildMappingProfile : Profile
{
    public GuildMappingProfile()
    {
        CreateMap<Guild, CreateGuildResponseDTO>().ReverseMap();
        CreateMap<Guild, GetGuildResponseDTO>().ReverseMap();
        CreateMap<Guild, GetAllGuildsResponseDTO>()
            .ForMember(dest => dest.NumberOfMembers, opt => opt.MapFrom(src => src.GuildMembers.Count))
            .ForMember(dest => dest.NumberOfParties, opt => opt.MapFrom(src => src.Parties.Count));

        // OLD
        CreateMap<Guild, CreateGuildRequestDTO>().ReverseMap();
        CreateMap<Guild, UpdateGuildRequestDTO>().ReverseMap();

        CreateMap<Guild, GuildResponseDTO>().ReverseMap();
        CreateMap<Guild, GetAllGuildsResponseDTO>().ReverseMap();
        CreateMap<Guild, UpdateGuildDetailsResponseDTO>().ReverseMap();
        CreateMap<Guild, UpdateGuildLeaderResponseDTO>().ReverseMap();

    }
}
