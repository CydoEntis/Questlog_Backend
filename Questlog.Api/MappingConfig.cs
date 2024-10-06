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
        // User mappings
        CreateMap<UserDTO, ApplicationUser>().ReverseMap();

        // MainQuest mappings
        CreateMap<MainQuest, CreateMainQuestRequestDTO>().ReverseMap();
        CreateMap<MainQuest, UpdateMainQuestRequestDTO>().ReverseMap();
        CreateMap<MainQuest, MainQuestResponseDTO>().ReverseMap();

        // QuestBoard mappings
        CreateMap<QuestBoard, CreateQuestBoardRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, UpdateQuestBoardRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, UpdateQuestBoardOrderRequestDTO>().ReverseMap();
        CreateMap<QuestBoard, QuestBoardResponseDTO>().ReverseMap();

        // Quest mappings
        CreateMap<Quest, CreateQuestRequestDTO>().ReverseMap();
        CreateMap<Quest, UpdateQuestRequestDTO>().ReverseMap();
        CreateMap<Quest, UpdateQuestOrderRequestDTO>().ReverseMap();
        CreateMap<Quest, QuestResponseDTO>().ReverseMap();
        CreateMap<Quest, QuestRequestDTO>().ReverseMap();

        // Guild mappings
        CreateMap<Guild, CreateGuildRequestDTO>().ReverseMap();

        CreateMap<Guild, GuildResponseDTO>()
            .ForMember(dest => dest.Parties, opt => opt.MapFrom(src => src.Parties))
            .ReverseMap();

        CreateMap<Guild, CreatedGuildResponseDTO>()
            .ForMember(dest => dest.GuildMembers, opt => opt.Ignore()) // Guild members are added in controller
            .ReverseMap();

        // GuildMember mappings
        CreateMap<GuildMember, GuildMemberResponseDTO>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ReverseMap();

        // Party mappings
        CreateMap<Party, CreatePartyRequestDTO>().ReverseMap();
        CreateMap<Party, PartyResponseDTO>()
            .ForMember(dest => dest.PartyMembers, opt => opt.MapFrom(src => src.PartyMembers))
            .ReverseMap();

        // Character mappings
        CreateMap<Character, CharacterResponseDTO>().ReverseMap();
        CreateMap<Character, CharacterRequestDTO>().ReverseMap();
    }
}
