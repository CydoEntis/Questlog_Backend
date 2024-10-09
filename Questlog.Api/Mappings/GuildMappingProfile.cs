using AutoMapper;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class GuildMappingProfile : Profile
    {
        public GuildMappingProfile()
        {
            CreateMap<Guild, CreateGuildRequestDTO>().ReverseMap();
            CreateMap<Guild, UpdateGuildRequestDTO>().ReverseMap();

            CreateMap<Guild, GuildResponseDTO>().ReverseMap();
            CreateMap<Guild, GetGuildResponseDTO>().ReverseMap();
            CreateMap<Guild, CreateGuildResponseDTO>().ReverseMap();
            CreateMap<Guild, UpdateGuildResponseDTO>().ReverseMap();
        }
    }
}
