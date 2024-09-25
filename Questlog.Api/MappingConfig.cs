using AutoMapper;
using Questlog.Application.Common.DTOs;
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
            CreateMap<QuestBoard, QuestBoardResponseDTO>().ReverseMap();

            CreateMap<Quest, QuestRequestDTO>().ReverseMap();
        }
    }
}
