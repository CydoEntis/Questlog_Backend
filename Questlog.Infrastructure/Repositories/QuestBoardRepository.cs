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
    public class QuestBoardRepository : BaseRepository<QuestBoard>, IQuestBoardRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestBoardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<QuestBoard> UpdateAsync(QuestBoard entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.QuestBoards.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<QuestBoard>> UpdateRangeAsync(List<QuestBoard> entities)
        {
            _db.QuestBoards.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
