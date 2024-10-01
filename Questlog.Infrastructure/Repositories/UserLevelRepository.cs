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
    public class UserLevelRepository : BaseRepository<UserLevel>, IUserLevelRepository
    {

        private readonly ApplicationDbContext _db;

        public UserLevelRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<UserLevel?> GetUserLevelByUserIdAsync(string userId)
        {
            return await GetAsync(x => x.ApplicationUserId == userId, tracked: true);
        }


    }
}
