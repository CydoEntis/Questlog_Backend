using AutoMapper;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class PartyMappingProfile : Profile
{
    public PartyMappingProfile()
    {
        CreateMap<Party, PartyDto>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Creator")
                .Select(m => m.User.Id)
                .FirstOrDefault()))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Creator")
                .Select(m => m.User.DisplayName)
                .FirstOrDefault()));


        CreateMap<Party, CreatePartyDto>().ReverseMap();
        CreateMap<Party, UpdatePartyDto>().ReverseMap();

        CreateMap<Party, PartyDto>()
            .ForMember(dest => dest.QuestStats, opt => opt.MapFrom(src => new QuestStatDto
            {
                TotalQuests = src.Quests.Count,
                CompletedQuests = src.Quests.Count(q => q.IsCompleted),
                InProgressQuests = src.Quests.Count(q => !q.IsCompleted),
                PastDueQuests = src.Quests.Count(q => q.DueDate < DateTime.Now)
            }));
    }
}