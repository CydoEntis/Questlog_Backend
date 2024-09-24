using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces
{
    public interface IMainQuestService
    {
        Task<MainQuest> GetMainQuest(string mainQuestId);
    }
}