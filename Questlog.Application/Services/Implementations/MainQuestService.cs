using AutoMapper;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Questlog.Application.Services.Implementations
{
    public class MainQuestService : IMainQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MainQuestService> _logger;

        public MainQuestService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MainQuestService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<MainQuest> GetMainQuest(int mainQuestId, string userId)
        {
            try
            {
                var mainQuest = await _unitOfWork.MainQuest.GetAsync(
                    mq => mq.Id == mainQuestId && mq.UserId == userId,
                    includeProperties: "QuestBoards");

                if (mainQuest == null)
                {
                    _logger.LogWarning($"MainQuest with ID {mainQuestId} not found for user {userId}.");
                    throw new KeyNotFoundException($"Could not find Main Quest with matching Id");
                }


                return mainQuest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Main Quest with ID {mainQuestId} for user {userId}.");
                throw;
            }
        }

        public async Task<IEnumerable<MainQuest>> GetAllMainQuestsForUser(string userId)
        {
            try
            {
                var mainQuests = await _unitOfWork.MainQuest.GetAllAsync(mq => mq.UserId == userId, includeProperties: "QuestBoards"); // Filter by UserId

                if (mainQuests == null || !mainQuests.Any())
                {
                    _logger.LogWarning($"No Main Quests found for user {userId}.");
                    return Enumerable.Empty<MainQuest>();
                }

                return mainQuests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving MainQuests for user {userId}.");
                throw;
            }
        }

        public async Task<int> CreateMainQuest(MainQuest mainQuest, string userId)
        {
            if (mainQuest == null)
            {
                throw new ArgumentNullException(nameof(mainQuest), "Main Quest cannot be null.");
            }

            try
            {
                mainQuest.UserId = userId;

                foreach (var questBoard in mainQuest.QuestBoards)
                {
                    questBoard.UserId = userId;
                }

                var newMainQuest = await _unitOfWork.MainQuest.CreateAsync(mainQuest);

                if (mainQuest.QuestBoards != null)

                {
                    foreach (var questBoard in mainQuest.QuestBoards)
                    {
                        questBoard.MainQuestId = newMainQuest.Id;
                    }

                    await _unitOfWork.SaveAsync();
                }

                return newMainQuest.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating Main Quest.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating MainQuest.");
                throw;
            }
        }

        public async Task<MainQuest> UpdateMainQuest(MainQuest mainQuest, string userId)
        {
            if (mainQuest == null)
            {
                throw new ArgumentNullException(nameof(mainQuest), "Main Quest cannot be null.");
            }

            try
            {
                var foundMainQuest = await _unitOfWork.MainQuest.GetAsync(
                    mq => mq.Id == mainQuest.Id && mq.UserId == userId, tracked: false);

                if (foundMainQuest == null)
                {
                    throw new KeyNotFoundException($"Main Quest with ID {mainQuest.Id} was not found for user {userId}.");
                }

                foundMainQuest.Title = mainQuest.Title;
                foundMainQuest.QuestColor = mainQuest.QuestColor;

                var updatedMainQuest = await _unitOfWork.MainQuest.UpdateAsync(foundMainQuest);
                return updatedMainQuest;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while updating Main Quest: {ex.Message}", ex);
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

        public async Task DeleteMainQuest(int id, string userId)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }

            try
            {
                var foundMainQuest = await _unitOfWork.MainQuest.GetAsync(
                    mq => mq.Id == id && mq.UserId == userId); 

                if (foundMainQuest == null)
                {
                    throw new KeyNotFoundException($"MainQuest with ID {id} was not found for user {userId}.");
                }

                await _unitOfWork.MainQuest.RemoveAsync(foundMainQuest);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while deleting Main Quest: {ex.Message}", ex);
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
