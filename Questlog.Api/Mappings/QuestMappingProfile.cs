using AutoMapper;
using Questlog.Application.Common.DTOs.Member.Response; // Make sure this DTO has member details
using Questlog.Application.Common.DTOs.MemberQuest.Response;
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
                opt => opt.MapFrom(src => src.MemberQuests.Count(mq => mq.AssignedQuestId == src.Id))) 
            .ForMember(dest => dest.TotalTasks,
                opt => opt.MapFrom(src => src.Tasks.Count))
            .ForMember(dest => dest.CompletedTasks,
                opt => opt.MapFrom(src => src.Tasks.Count(sq => sq.IsCompleted)))
            .ForMember(dest => dest.AssignedMembers, opt =>
                opt.MapFrom(src => src.MemberQuests
                    .Where(mq => mq.AssignedQuestId == src.Id) 
                    .Select(mq => new GetMemberResponseDto
                    {
                        Id = mq.AssignedMember.Id, 
                        UserId = mq.UserId, 
                        DisplayName = mq.AssignedMember.User.DisplayName, 
                        Email = mq.AssignedMember.User.Email, 
                        Avatar = mq.AssignedMember.User.Avatar, 
                        CurrentLevel = mq.AssignedMember.User.CurrentLevel, 
                        JoinedOn = mq.AssignedMember.JoinedOn, 
                    })));


        CreateMap<Quest, GetMemberQuestResponseDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestResponseDto>().ReverseMap();
    }
}