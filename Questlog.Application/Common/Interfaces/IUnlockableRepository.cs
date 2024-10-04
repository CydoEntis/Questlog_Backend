using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IUnlockableRepository : IBaseRepository<Unlockable>
    {
        Task<Unlockable> UpdateAsync(Unlockable entity);
    }
}
