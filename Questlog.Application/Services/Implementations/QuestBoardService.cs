using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class QuestBoardService : IQuestBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<QuestBoardService> _logger;

        public QuestBoardService(IUnitOfWork unitOfWork, ILogger<QuestBoardService> logger)
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

        public async Task<IEnumerable<QuestBoard>> GetAllQuestBoardsForUser(QuestBoardFilterParams filterParams, string userId)
        {
            try
            {
                IEnumerable<QuestBoard> questBoards;

                // Check if filterParams is null or does not contain filters
                if (filterParams == null || !filterParams.MainQuestId.HasValue)
                {
                    questBoards = await _unitOfWork.QuestBoard.GetAllAsync(
                        qb => qb.UserId == userId, includeProperties: "Quests");
                }
                else
                {
                    // Apply the filter for MainQuestId
                    questBoards = await _unitOfWork.QuestBoard.GetAllAsync(
                        qb => qb.MainQuestId == filterParams.MainQuestId && qb.UserId == userId, includeProperties: "Quests");
                }

                // Log a warning if no quest boards found
                if (questBoards == null || !questBoards.Any())
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
            if (questBoard is null)
            {
                throw new ArgumentNullException(nameof(questBoard), "Quest Board cannot be null");
            }

            try
            {
                questBoard.UserId = userId;
                var newQuestBoard = await _unitOfWork.QuestBoard.CreateAsync(questBoard);

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

        public async Task<QuestBoard> UpdateQuestBoard(QuestBoard questBoard, string userId)
        {
            if (questBoard == null)
            {
                throw new ArgumentNullException(nameof(questBoard), "Quest Board cannot be null.");
            }

            try
            {
                var foundQuestBoard = await _unitOfWork.QuestBoard.GetAsync(
                    mq => mq.Id == questBoard.Id && mq.UserId == userId, tracked: false);

                if (foundQuestBoard == null)
                {
                    throw new KeyNotFoundException($"Quest Board with ID {questBoard.Id} was not found for user {userId}.");
                }

                foundQuestBoard.Title = questBoard.Title;
                foundQuestBoard.BoardColor = questBoard.BoardColor;
                foundQuestBoard.Order = questBoard.Order;

                var updatedQuestBoard = await _unitOfWork.QuestBoard.UpdateAsync(foundQuestBoard);
                return updatedQuestBoard;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while updating Quest Board: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while updating the database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<QuestBoard>> UpdateQuestBoardsOrder(List<QuestBoard> questBoards, string userId)
        {
            if (questBoards == null || !questBoards.Any())
            {
                throw new ArgumentNullException(nameof(questBoards), "Quest Boards cannot be null or empty.");
            }

            try
            {
                var questBoardIds = questBoards.Select(qb => qb.Id).ToList();

                var foundQuestBoards = await _unitOfWork.QuestBoard.GetAllAsync(
                    qb => questBoardIds.Contains(qb.Id) && qb.UserId == userId);

                if (foundQuestBoards == null || !foundQuestBoards.Any())
                {
                    throw new KeyNotFoundException($"No quest boards were found for the given user {userId}.");
                }

                foreach (var foundQuestBoard in foundQuestBoards)
                {
                    var updatedQuestBoard = questBoards.First(qb => qb.Id == foundQuestBoard.Id);
                    foundQuestBoard.Order = updatedQuestBoard.Order;
                }

                var updatedQuestBoards = await _unitOfWork.QuestBoard.UpdateRangeAsync(foundQuestBoards);

                return updatedQuestBoards;  
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while updating quest boards: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while updating the database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task DeleteQuestBoard(int id, string userId)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }

            try
            {
                var foundQuestBoard = await _unitOfWork.QuestBoard.GetAsync(
                    mq => mq.Id == id && mq.UserId == userId);

                if (foundQuestBoard == null)
                {
                    throw new KeyNotFoundException($"Quest Board with ID {id} was not found for user {userId}.");
                }

                await _unitOfWork.QuestBoard.RemoveAsync(foundQuestBoard);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while deleting Quest Board: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while deleting from the database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
