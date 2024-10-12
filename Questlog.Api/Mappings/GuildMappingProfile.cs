using AutoMapper;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.ApplicationUser.Response;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class GuildMappingProfile : Profile
    {
        public GuildMappingProfile()
        {
            CreateMap<Guild, CreateGuildResponseDTO>().ReverseMap();

            


            // OLD
            CreateMap<Guild, CreateGuildRequestDTO>().ReverseMap();
            CreateMap<Guild, UpdateGuildRequestDTO>().ReverseMap();

            CreateMap<Guild, GuildResponseDTO>().ReverseMap();
            CreateMap<Guild, GetGuildResponseDTO>().ReverseMap();
            CreateMap<Guild, GetAllGuildsResponseDTO>().ReverseMap();
            CreateMap<Guild, UpdateGuildDetailsResponseDTO>().ReverseMap();
            CreateMap<Guild, UpdateGuildLeaderResponseDTO>().ReverseMap();

        }
    }
}
