using AutoMapper;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.DTOs.Character;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.PartyMember;
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
        CreateMap<CreateGuildRequestDTO, Guild>().ReverseMap();
        CreateMap<Guild, CreateGuildResponseDTO>().ReverseMap();

        // Guild Member Mappings
        CreateMap<GuildMember, GuildMemberResponseDTO>().ReverseMap();


        // Party Mappings
        CreateMap<CreatePartyRequestDTO, Party>().ReverseMap();
        CreateMap<Party, CreatePartyResponseDTO>().ReverseMap();

        // Party Member Mappings
        CreateMap<PartyMember, CreatePartyMemberResponseDTO>().ReverseMap();



        // Character mappings
        CreateMap<Character, CharacterResponseDTO>().ReverseMap();
        CreateMap<Character, CharacterRequestDTO>().ReverseMap();
    }
}
