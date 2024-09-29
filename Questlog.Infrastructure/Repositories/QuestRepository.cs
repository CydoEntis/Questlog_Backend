
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
    public class QuestRepository : BaseRepository<Quest>, IQuestRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Quest> UpdateAsync(Quest entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Quests.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Quest>> UpdateRangeAsync(List<Quest> entities)
        {
            _db.Quests.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
