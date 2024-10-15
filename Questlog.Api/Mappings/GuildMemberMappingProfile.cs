using AutoMapper;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class GuildMemberMappingProfile : Profile
{
    public GuildMemberMappingProfile()
    {

        CreateMap<GuildMember, GetGuildMemberResponseDTO>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.User.CurrentLevel));


        // Old
        CreateMap<GuildMember, GuildMemberResponseDTO>().ReverseMap();
        CreateMap<GuildMember, CreateGuildMemberRequestDTO>().ReverseMap();
        CreateMap<GuildMember, CreateGuildMemberResponseDTO>().ReverseMap();


    }
}
