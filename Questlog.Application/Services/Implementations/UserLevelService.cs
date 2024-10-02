using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class UserLevelService : IUserLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserLevelService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserLevel> GetUserLevelAsync(string userId)
        {
            try
            {
                var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);

                if (userLevel is null)
                {
                    _logger.LogWarning($"User Level not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find User Level for User with id {userId}");
                }

                return userLevel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving User Level for user with id: {userId}");
                throw;
            }

        }

        public async Task<UserLevel> CreateDefaultUserLevelAsync(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");
            }

            try
            {
                var userLevel = new UserLevel
                {
                    ApplicationUserId = userId,
                    CurrentLevel = 1,
                    CurrentExp = 0
                };

                await _unitOfWork.UserLevel.CreateAsync(userLevel);

                return userLevel;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating User Level.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating User Level.");
                throw;
            }


        }


        public async Task AddExpAsync(string userId, string priority)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");

            }

            if (string.IsNullOrEmpty(priority))
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null");
            }

            try
            {
                var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);
                if (userLevel is null)
                {
                    _logger.LogWarning($"User Level not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find User Level for User with id {userId}");
                }
                int expToAdd = GetExpForPriority(priority);

                userLevel.CurrentExp += expToAdd;

                while (userLevel.CurrentExp >= userLevel.ExpForNextLevel)
                {
                    userLevel.CurrentExp -= userLevel.ExpForNextLevel;
                    userLevel.CurrentLevel++;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding exp to UserLevel.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding exp to user level.");
                throw;
            }
        }

        public async Task RemoveExpAsync(string userId, string priority)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");

            }

            if (string.IsNullOrEmpty(priority))
            {
                throw new ArgumentNullException(nameof(priority), "Priority cannot be null");
            }

            try
            {
                var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);
                if (userLevel is null)
                {
                    _logger.LogWarning($"User Level not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find User Level for User with id {userId}");
                }
                int expToDeduct = GetExpForPriority(priority);

                int newExp = userLevel.CurrentExp - expToDeduct;

                while (newExp < 0 && userLevel.CurrentLevel > 1)
                {
                    userLevel.CurrentLevel--;
                    newExp += userLevel.CalculateExpForLevel();
                }

                userLevel.CurrentExp = Math.Max(0, newExp);

                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while removing exp from UserLevel.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing exp from user level.");
                throw;
            }



        }


        private int GetExpForPriority(string priority)
        {
            return priority.ToLower() switch
            {
                "low" => 5,
                "medium" => 10,
                "high" => 15,
                "urgent" => 20,
                _ => 0
            };
        }
    }
}
