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
    public class MainQuestRepository : BaseRepository<MainQuest>, IMainQuestRepository
    {
        private readonly ApplicationDbContext _db;

        public MainQuestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<MainQuest> UpdateAsync(MainQuest entity)
        {
            throw new NotImplementedException();
        }
    }
}
