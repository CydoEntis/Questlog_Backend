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

        public Task<Quest> UpdateAsync(Quest entity)
        {
            throw new NotImplementedException();
        }
    }
}
