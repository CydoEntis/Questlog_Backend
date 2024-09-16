using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _db;

        public TokenRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshToken> GetRefreshToken(string refreshToken)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(token => token.Refresh_Token == refreshToken);
        }

        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task InvalidateAllUsersTokens(string userId, string tokenId)
        {
            await _db.RefreshTokens.Where(token => token.UserId == userId && token.JwtTokenId == tokenId).ExecuteUpdateAsync(token => token.SetProperty(refreshToken => refreshToken.IsValid, false));
        }
    }
}
