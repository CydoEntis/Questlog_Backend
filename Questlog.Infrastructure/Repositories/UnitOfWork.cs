using Questlog.Application.Common.Interfaces;
using Questlog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IUserRepository User { get; private set; }
        public ITokenRepository Token { get; private set; }
        public IMainQuestRepository MainQuest { get; private set; }
        public IQuestBoardRepository QuestBoard { get; private set; }
        public IQuestRepository Quest { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(db);
            Token = new TokenRepository(db); 
            MainQuest = new MainQuestRepository(db);
            QuestBoard = new QuestBoardRepository(db);
            Quest = new QuestRepository(db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }

        public void SaveAsync()
        {
            _db.SaveChangesAsync();
        }
    }
}
