using AutoMapper;
using Questlog.Application.Common.DTOs.MemberQuest;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class MemberQuestMappingProfile : Profile
{
    public MemberQuestMappingProfile()
    {
        CreateMap<MemberQuest, MemberQuestDto>().ReverseMap();
    }
}