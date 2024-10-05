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


        public ICharacterRepository Character { get; private set; }
        public IUnlockableRepository Unlockable { get; private set; }
        public IAdventureRepository Adventure { get; private set; }
        public IPartyRepository Party { get; private set; }
        public IPartyMemberRepository PartyMember { get; private set; }




        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            User = new UserRepository(db);
            Token = new TokenRepository(db);


            Adventure = new AdventureRepository(db);
            Party = new PartyRepository(db);
            PartyMember = new PartyMemberRepository(db);

            Character = new CharacterRepository(db);
            Unlockable = new UnlockableRepository(db);

            // TODO: Still need  to be reworked/refactored
            MainQuest = new MainQuestRepository(db);
            QuestBoard = new QuestBoardRepository(db);
            Quest = new QuestRepository(db);


 
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
