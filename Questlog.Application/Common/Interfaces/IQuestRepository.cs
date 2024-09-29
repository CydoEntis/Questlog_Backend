using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IQuestRepository : IBaseRepository<Quest>
    {
        Task<Quest> UpdateAsync(Quest entity);
        Task<List<Quest>> UpdateRangeAsync(List<Quest> entities);
    }
}
