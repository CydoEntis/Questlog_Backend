using Microsoft.EntityFrameworkCore;
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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<bool> isUserUnique(string email)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(user => user.UserName == email);
            return user is null;
        }

        //public Task RefreshAccessToken(TokenDTO tokenDTO)
        //{
        //    throw new NotImplementedException();
        //}


        //public Task RevokeRefreshToken(TokenDTO tokenDTO)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
