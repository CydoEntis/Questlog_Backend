﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs;
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
    public class QuestService : IQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Quest> _logger;

        public QuestService(IUnitOfWork unitOfWork, ILogger<Quest> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Quest> GetQuest(int questId, string userId)
        {
            try
            {
                var quest = await _unitOfWork.Quest.GetAsync(qb => qb.Id == questId && qb.UserId == userId);

                if (quest is null)
                {
                    _logger.LogWarning($"Quest with ID {questId} not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not found Quest with matching Id");
                }

                return quest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving Quest with ID {questId} for user {userId}");
                throw;
            }
        }

        public async Task<IEnumerable<Quest>> GetAllQuestsForUser(string userId)
        {
            try
            {
                var quests = await _unitOfWork.Quest.GetAllAsync(qb => qb.UserId == userId);

                if (quests is null || !quests.Any())
                {
                    _logger.LogWarning($"No Quest s found for user with ID {userId}");
                    return Enumerable.Empty<Quest>();
                }

                return quests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Quests for user with id {userId}");
                throw;
            }

        }

        public async Task<int> CreateQuest(Quest quest, string userId)
        {
            if (quest is null)
            {
                throw new ArgumentNullException(nameof(quest), "Quest cannot be null");
            }

            try
            {
                quest.UserId = userId;
                var newQuest = await _unitOfWork.Quest.CreateAsync(quest);

                return newQuest.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating Quest.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating Quest .");
                throw;
            }
        }

        public async Task<Quest> UpdateQuest(Quest quest, string userId)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest), "Quest cannot be null.");
            }

            try
            {
                var foundQuest = await _unitOfWork.Quest.GetAsync(
                    mq => mq.Id == quest.Id && mq.UserId == userId, tracked: false);

                if (foundQuest == null)
                {
                    throw new KeyNotFoundException($"Quest  with ID {quest.Id} was not found for user {userId}.");
                }

                foundQuest.Title = quest.Title;
                foundQuest.Desc = quest.Desc;
                foundQuest.Items = quest.Items;
                foundQuest.Priority = quest.Priority;
                foundQuest.Order = quest.Order;
                foundQuest.Completed = quest.Completed;

                var updatedQuest = await _unitOfWork.Quest.UpdateAsync(foundQuest);
                return updatedQuest;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while updating Quest : {ex.Message}", ex);
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

        public async Task DeleteQuest(int id, string userId)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }

            try
            {
                var foundQuest = await _unitOfWork.Quest.GetAsync(
                    mq => mq.Id == id && mq.UserId == userId);

                if (foundQuest == null)
                {
                    throw new KeyNotFoundException($"Quest  with ID {id} was not found for user {userId}.");
                }

                await _unitOfWork.Quest.RemoveAsync(foundQuest);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception($"A concurrency error occurred while deleting Quest : {ex.Message}", ex);
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