using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface ICharacterService
    {
        Task<Character> GetCharacterAsync(string userId);
        Task<Character> CreateCharacterAsync(string userId, Character character);
        Task AddExpAsync(string userId, string priority);
        Task RemoveExpAsync(string userId, string priority);
    }
}
