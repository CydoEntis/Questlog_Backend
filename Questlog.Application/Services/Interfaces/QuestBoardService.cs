using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public class QuestBoardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestBoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateQuestBoard(QuestBoard questBoard)
        {
            var newQuestBoard = await _unitOfWork.QuestBoard.CreateAsync(questBoard);
            return newQuestBoard.Id;
        }
    }
}
