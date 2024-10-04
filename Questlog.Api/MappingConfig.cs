using AutoMapper;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.DTOs.Character;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Application.Common.DTOs.UserLevel;
using Questlog.Domain.Entities;

namespace Questlog.Api
{
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

            CreateMap<UserLevel, UserLevelResponseDTO>().ReverseMap();

            CreateMap<Character, CharacterResponseDTO>().ReverseMap();
            CreateMap<Character, CharacterRequestDTO>().ReverseMap();



        }
    }
}
