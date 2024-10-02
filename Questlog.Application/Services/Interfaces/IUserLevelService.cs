using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IUserLevelService
    {
        Task<UserLevel> GetUserLevelAsync(string userId);
        Task AddExpAsync(string userId, string priority);
        Task RemoveExpAsync(string userId, string priority);
    }
}
