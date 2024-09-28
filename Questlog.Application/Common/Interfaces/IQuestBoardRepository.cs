using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IQuestBoardRepository : IBaseRepository<QuestBoard>
    {
        Task<QuestBoard> UpdateAsync(QuestBoard entity);
        Task<List<QuestBoard>> UpdateRangeAsync(List<QuestBoard> entities);
    }
}
