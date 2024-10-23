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

        CreateMap<Quest, GetQuestResponseDto>()
            .ForMember(dest => dest.TotalMembers,
                opt => opt.MapFrom(src => src.AssignedMembers.Count))
            .ForMember(dest => dest.TotalTasks,
                opt => opt.MapFrom(src => src.Tasks.Count))
            .ForMember(dest => dest.CompletedTasks,
                opt => opt.MapFrom(src => src.Tasks.Count(sq => sq.IsCompleted)));


        CreateMap<Quest, UpdateQuestRequestDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestResponseDto>().ReverseMap();
    }
}