using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IUserLevelService
    {
        Task AddExpAsync(string userId, string priority);
        Task RemoveExpAsync(string userId, string priority);
    }
}
