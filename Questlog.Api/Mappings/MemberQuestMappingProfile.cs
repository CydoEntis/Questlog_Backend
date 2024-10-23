using AutoMapper;
using Questlog.Application.Common.DTOs.MemberQuest.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class MemberQuestMappingProfile : Profile
{
    public MemberQuestMappingProfile()
    {
        CreateMap<MemberQuest, GetMemberQuestResponseDto>().ReverseMap();
    }
}