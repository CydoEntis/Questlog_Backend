using AutoMapper;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.DTOs.GuildMember.Response;
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

            CreateMap<GuildMember, GetGuildMemberResponseDTO>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Character.Avatar))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Character.DisplayName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Character.UserId))
            .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.Character.CurrentLevel));
        }
    }
}
