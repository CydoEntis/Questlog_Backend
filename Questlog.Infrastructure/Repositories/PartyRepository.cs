
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
    public class PartyRepository : BaseRepository<Party>, IPartyRepository
    {
        private readonly ApplicationDbContext _db;

        public PartyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Party> UpdateAsync(Party entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Parties.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        //public async Task<List<Party>> UpdateRangeAsync(List<Party> entities)
        //{
        //    _db.Partys.UpdateRange(entities);
        //    await _db.SaveChangesAsync();
        //    return entities;
        //}
    }
}
