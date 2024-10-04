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
    public class UnlockableRepository : BaseRepository<Unlockable>, IUnlockableRepository
    {
        private readonly ApplicationDbContext _db;

        public UnlockableRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Unlockable> UpdateAsync(Unlockable entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Unlockables.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
