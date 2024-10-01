using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        ITokenRepository Token { get; }
        IMainQuestRepository MainQuest { get; }
        IQuestBoardRepository QuestBoard { get; }
        IQuestRepository Quest { get; }
        IUserLevelRepository UserLevel { get; }


        void Save();

        Task SaveAsync();
    }

}
