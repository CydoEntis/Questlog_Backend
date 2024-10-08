
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
    public class PartyMemberRepository : BaseRepository<PartyMember>, IPartyMemberRepository
    {
        private readonly ApplicationDbContext _db;

        public PartyMemberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PartyMember> UpdateAsync(PartyMember entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.PartyMembers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }


    }
}
