using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<QuestBoard>> GetAllQuestsBoardsForUser(string userId)
        {
            try
            {
                var questBoards = await _unitOfWork.QuestBoard.GetAllAsync(qb => qb.UserId == userId, includeProperties: "Quests");

                if (questBoards is null || !questBoards.Any())
                {
                    _logger.LogWarning($"No Quest Boards found for user with ID {userId}");
                    return Enumerable.Empty<QuestBoard>();
                }

                return questBoards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Quest Boards for user with id {userId}");
                throw;
            }

        }




        public async Task<int> CreateQuestBoard(QuestBoard questBoard, string userId)
        {
            if(questBoard is null)
            {
                throw new ArgumentNullException(nameof(questBoard), "Quest Board cannot be null");    
            }

            try
            {
                questBoard.UserId = userId;
                var newQuestBoard = _unitOfWork.QuestBoard.CreateAsync(questBoard);

                return newQuestBoard.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating Quest Board.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating Quest Board.");
                throw;
            }
        }
    }
}
