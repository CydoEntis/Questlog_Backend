using AutoMapper;
using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.DTOs.MemberQuest.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class MemberQuestMappingProfile : Profile
{
    public MemberQuestMappingProfile()
    {
        CreateMap<MemberQuest, GetMemberQuestResponseDto>().ReverseMap();
        CreateMap<MemberQuest, GetMemberResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssignedMember.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.AssignedMember.User.DisplayName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AssignedMember.User.Email))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AssignedMember.User.Avatar))
            .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.AssignedMember.User.CurrentLevel))
            .ForMember(dest => dest.JoinedOn, opt => opt.MapFrom(src => src.AssignedMember.JoinedOn))
            .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => src.AssignedQuest.CampaignId));
    }
}