using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces
{
    public interface IMainQuestService
    {
        Task<IEnumerable<MainQuest>> GetAllMainQuestsForUser(string userId);
        Task<MainQuest> GetMainQuest(int mainQuestId, string userId);
        Task<MainQuest> CreateMainQuest(MainQuest mainQuest, string userId);
        Task<MainQuest> UpdateMainQuest(MainQuest mainQuest, string userId);
        Task DeleteMainQuest(int id, string userId);
    }
}