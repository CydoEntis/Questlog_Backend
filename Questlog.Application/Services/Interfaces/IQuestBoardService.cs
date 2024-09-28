using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Application.Common.Enums;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces
{
    public interface IQuestBoardService
    {
        Task<IEnumerable<QuestBoard>> GetAllQuestBoardsForUser(QuestBoardFilterParams filterParams,
            string userId);
        Task<QuestBoard> GetQuestBoard(int questBoardId, string userId);
        Task<int> CreateQuestBoard(QuestBoard questBoard, string userId);
        Task<QuestBoard> UpdateQuestBoard(QuestBoard questBoard, string userId);
        Task<List<QuestBoard>> UpdateQuestBoardsOrder(List<QuestBoard> questBoards, string userId);
        Task DeleteQuestBoard(int id, string userId);

    }
}