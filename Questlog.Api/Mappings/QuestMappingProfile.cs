using AutoMapper;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.Quest.Request;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class QuestMappingProfile : Profile
{
    public QuestMappingProfile()
    {
        CreateMap<Quest, CreateQuestResponseDto>().ReverseMap();
        CreateMap<Quest, CreateQuestRequestDto>().ReverseMap();
        CreateMap<Quest, GetQuestResponseDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestResponseDto>().ReverseMap();
    }
}