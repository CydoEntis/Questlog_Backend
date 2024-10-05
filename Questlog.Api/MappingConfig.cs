using AutoMapper;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.DTOs.Character;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Domain.Entities;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<UserDTO, ApplicationUser>().ReverseMap();
        CreateMap<MainQuest, CreateMainQuestRequestDTO>().ReverseMap();
        CreateMap<MainQuest, UpdateMainQuestRequestDTO>().ReverseMap();
        CreateMap<MainQuest, MainQuestResponseDTO>().ReverseMap();

        CreateMap<QuestBoard, CreateQuestBoardRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, UpdateQuestBoardRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, UpdateQuestBoardOrderRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, QuestBoardResponseDTO>().ReverseMap();

        CreateMap<Quest, CreateQuestRequestDTO>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDTO>().ReverseMap();
        CreateMap<Quest, UpdateQuestOrderRequestDTO>().ReverseMap();
        CreateMap<Quest, QuestResponseDTO>().ReverseMap();
        CreateMap<Quest, QuestRequestDTO>().ReverseMap();

        CreateMap<Guild, CreateGuildRequestDTO>().ReverseMap();
        CreateMap<Guild, GuildResponseDTO>()
            .ForMember(dest => dest.Parties, opt => opt.MapFrom(src => src.Parties))
            .ReverseMap(); 

        CreateMap<Party, CreatePartyRequestDTO>().ReverseMap();
        CreateMap<Party, PartyResponseDTO>()
            .ForMember(dest => dest.PartyMembers, opt => opt.MapFrom(src => src.PartyMembers))
            .ReverseMap(); 

        CreateMap<Character, CharacterResponseDTO>().ReverseMap();
        CreateMap<Character, CharacterRequestDTO>().ReverseMap();
    }
}
