using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories
{
    public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
    {
        private readonly ApplicationDbContext _db;

        public AvatarRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Avatar>> GetAvatarsAtNextUnlockableLevelAsync(int userLevel)
        {
            var nextUnlockLevel = await _db.Avatars
                .Where(a => a.UnlockLevel > userLevel)
                .OrderBy(a => a.UnlockLevel)
                .Select(a => a.UnlockLevel)
                .FirstOrDefaultAsync();

            if (nextUnlockLevel == 0)
            {
                return new List<Avatar>();
            }

            return await _db.Avatars
                .Where(a => a.UnlockLevel == nextUnlockLevel)
                .ToListAsync();
        }
    }
}