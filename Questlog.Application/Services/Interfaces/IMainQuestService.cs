using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces
{
    public interface IMainQuestService
    {
        Task<MainQuest> GetMainQuest(int mainQuestId);
        Task<int> CreateMainQuest(MainQuest mainQuest);
        Task<MainQuest> UpdateMainQuest(MainQuest mainQuest);
    }
}