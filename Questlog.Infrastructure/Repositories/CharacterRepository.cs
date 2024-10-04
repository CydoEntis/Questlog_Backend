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
    public class CharacterRepository : BaseRepository<Character>, ICharacterRepository
    {
        private readonly ApplicationDbContext _db;

        public CharacterRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Character> UpdateAsync(Character entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.MainQuests.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
