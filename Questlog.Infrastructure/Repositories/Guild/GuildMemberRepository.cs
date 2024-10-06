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
    public class GuildRepository : BaseRepository<Guild>, IGuildRepository
    {
        private readonly ApplicationDbContext _db;

        public GuildRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Guild> UpdateAsync(Guild entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Guilds.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}