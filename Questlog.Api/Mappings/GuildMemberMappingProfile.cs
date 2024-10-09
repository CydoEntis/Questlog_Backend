using AutoMapper;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class GuildMemberMappingProfile : Profile
    {
        public GuildMemberMappingProfile()
        {
            CreateMap<GuildMember, GuildMemberResponseDTO>()
                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.Character))
                .ReverseMap();
            CreateMap<GuildMember, CreateGuildMemberRequestDTO>().ReverseMap();
            CreateMap<GuildMember, CreateGuildMemberResponseDTO>().ReverseMap();
        }
    }
}
