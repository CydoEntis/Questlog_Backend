using AutoMapper;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class QuestMappingProfile : Profile
    {
        public QuestMappingProfile()
        {
            // Quest mappings
            CreateMap<Quest, CreateQuestRequestDTO>().ReverseMap();
            CreateMap<Quest, UpdateQuestRequestDTO>().ReverseMap();
            CreateMap<Quest, UpdateQuestOrderRequestDTO>().ReverseMap();
            CreateMap<Quest, QuestResponseDTO>().ReverseMap();
            CreateMap<Quest, QuestRequestDTO>().ReverseMap();

            // QuestBoard mappings
            CreateMap<QuestBoard, CreateQuestBoardRequestDTO>().ReverseMap();
            CreateMap<QuestBoard, UpdateQuestBoardRequestDTO>().ReverseMap();
            CreateMap<QuestBoard, UpdateQuestBoardOrderRequestDTO>().ReverseMap();
            CreateMap<QuestBoard, QuestBoardResponseDTO>().ReverseMap();
        }
    }
}
