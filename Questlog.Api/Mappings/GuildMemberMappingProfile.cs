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
            CreateMap<GuildMember, GuildMemberResponseDTO>().ReverseMap();
            CreateMap<GuildMember, CreateGuildMemberRequestDTO>().ReverseMap();
            CreateMap<GuildMember, CreateGuildMemberResponseDTO>().ReverseMap();

            CreateMap<GuildMember, GetGuildMemberResponseDTO>().ReverseMap();

        }
    }
}
