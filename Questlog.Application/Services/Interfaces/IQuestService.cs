using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces
{
    public interface IQuestService
    {
        Task<IEnumerable<Quest>> GetAllQuestsForUser(string userId);
        Task<Quest> GetQuest(int questId, string userId);
        Task<int> CreateQuest(Quest quest, string userId);
        Task<Quest> UpdateQuest(Quest quest, string userId);
        Task<List<Quest>> UpdateQuestsOrderInQuestBoard(List<Quest> quests, string userId);
        Task DeleteQuest(int id, string userId);
    }
}