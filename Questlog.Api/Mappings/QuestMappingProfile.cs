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
            .ForMember(dest => dest.CompletedSteps,
                opt => opt.MapFrom(src => src.Steps.Count(sq => sq.IsCompleted)))
            .ForMember(dest => dest.Members, opt =>
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
                        CampaignId = mq.AssignedQuest.CampaignId
                    })));


        CreateMap<Quest, GetMemberQuestResponseDto>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDto>().ReverseMap();


        CreateMap<Quest, UpdateQuestResponseDto>()
            .ForMember(dest => dest.CompletedSteps,
                opt => opt.MapFrom(src => src.Steps.Count(sq => sq.IsCompleted)))
            .ForMember(dest => dest.Members, opt =>
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
                        CampaignId = mq.AssignedQuest.CampaignId
                    })));
    }
}