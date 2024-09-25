using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs;
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
        private readonly ILogger _logger;

        public QuestBoardService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<QuestBoard> GetQuestBoard(int questBoardId, string userId)
        {
            try
            {
                var questBoard = await _unitOfWork.QuestBoard.GetAsync(qb => qb.Id == questBoardId && qb.UserId == userId, includeProperties: "Quests");

                if (questBoard is null)
                {
                    _logger.LogWarning($"Quest Board with ID {questBoardId} not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not found Quest Board with matching Id");
                }

                return questBoard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving Quest Board with ID {questBoardId} for user {userId}");
                throw;
            }
        }


        public async Task<int> CreateQuestBoard(QuestBoard questBoard)
        {
            var newQuestBoard = await _unitOfWork.QuestBoard.CreateAsync(questBoard);
            return newQuestBoard.Id;
        }
    }
}
