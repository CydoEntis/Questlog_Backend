using Questlog.Application.Common.DTOs;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetRefreshToken(string refreshToken);
        Task AddRefreshToken(RefreshToken refreshToken);
        Task InvalidateAllUsersTokens(string userId, string tokenId);
    }
}
