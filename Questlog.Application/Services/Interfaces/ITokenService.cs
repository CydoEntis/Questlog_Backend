using Questlog.Application.Common.DTOs;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(ApplicationUser user, string tokeId);
        Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
        Task<string> CreateRefreshToken(string userId, string tokenId);
        void InvalidateToken(RefreshToken refreshToken);
        Task RevokeRefreshToken(TokenDTO tokenDTO);
    }
}
