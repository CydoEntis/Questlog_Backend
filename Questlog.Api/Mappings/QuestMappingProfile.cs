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
                opt => opt.MapFrom(src => src.MemberQuests.Count(mq => mq.AssignedQuestId == src.Id))) // Count matching MemberQuests
            .ForMember(dest => dest.TotalTasks,
                opt => opt.MapFrom(src => src.Tasks.Count))
            .ForMember(dest => dest.CompletedTasks,
                opt => opt.MapFrom(src => src.Tasks.Count(sq => sq.IsCompleted)))
            .ForMember(dest => dest.AssignedMembers, opt =>
                opt.MapFrom(src => src.MemberQuests
                    .Where(mq => mq.AssignedQuestId == src.Id) // Filter MemberQuests to match QuestId
                    .Select(mq => new GetMemberResponseDto
                    {
                        Id = mq.AssignedMember.Id, // Member's ID
                        UserId = mq.UserId, // User ID from MemberQuest
                        DisplayName = mq.AssignedMember.User.DisplayName, // Member's user display name
                        Email = mq.AssignedMember.User.Email, // Member's email
                        Avatar = mq.AssignedMember.User.Avatar, // Member's avatar
                        CurrentLevel = mq.AssignedMember.User.CurrentLevel, // Member's level
                        JoinedOn = mq.AssignedMember.JoinedOn, // Member's join date
                    })));


        CreateMap<Quest, GetMemberQuestResponseDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestResponseDto>().ReverseMap();
    }
}