using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IUserLevelRepository
    {
        Task<UserLevel?> GetUserLevelByUserIdAsync(string userId);
    }
}
