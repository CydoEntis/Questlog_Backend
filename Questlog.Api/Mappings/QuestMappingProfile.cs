using AutoMapper;
using Questlog.Application.Common.DTOs.Avatar; // Make sure this is included
using Questlog.Application.Common.DTOs.Member;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class QuestMappingProfile : Profile
    {
        public QuestMappingProfile()
        {
            CreateMap<Quest, CreateQuestDto>().ReverseMap();
            CreateMap<Quest, UpdateQuestDto>().ReverseMap();

            CreateMap<Quest, QuestDto>()
                .ForMember(dest => dest.CompletedSteps,
                    opt => opt.MapFrom(src => src.Steps.Count(sq => sq.IsCompleted)))
                .ForMember(dest => dest.Members, opt =>
                    opt.MapFrom(src => src.MemberQuests
                        .Where(mq => mq.AssignedQuestId == src.Id)
                        .Select(mq => new MemberDto()
                        {
                            Id = mq.AssignedMember.Id,
                            CampaignId = mq.AssignedQuest.PartyId,
                            Role = mq.AssignedMember.Role,
                            UserId = mq.UserId,
                            DisplayName = mq.AssignedMember.User.DisplayName,
                            Email = mq.AssignedMember.User.Email,
                            Avatar = new AvatarDto
                            {
                                Id = mq.AssignedMember.User.Avatar.Id,
                                Name = mq.AssignedMember.User.Avatar.Name,
                            },
                            CurrentLevel = mq.AssignedMember.User.CurrentLevel,
                            JoinedOn = mq.AssignedMember.JoinedOn,
                        })));
        }
    }
}