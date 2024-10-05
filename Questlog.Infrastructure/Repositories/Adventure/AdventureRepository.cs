
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
    public class AdventureRepository : BaseRepository<Adventure>, IAdventureRepository
    {
        private readonly ApplicationDbContext _db;

        public AdventureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Adventure> UpdateAsync(Adventure entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Adventures.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
