using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IAdventureRepository : IBaseRepository<Adventure>
    {
        Task<Adventure> UpdateAsync(Adventure entity);
    }
}
