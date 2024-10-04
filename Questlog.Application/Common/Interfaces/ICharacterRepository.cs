using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface ICharacterRepository : IBaseRepository<Character>
    {
        Task<Character?> GetCharacterAsync(string userId);
        Task<Character> UpdateAsync(Character entity);
    }
}
