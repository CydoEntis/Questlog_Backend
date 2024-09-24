using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IMainQuestRepository : IBaseRepository<MainQuest>
    {
        Task<MainQuest> UpdateAsync(MainQuest entity);
    }
}
