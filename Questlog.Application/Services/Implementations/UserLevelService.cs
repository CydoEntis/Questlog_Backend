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

        public UserLevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<UserLevel> CreateDefaultUserLevelAsync(string applicationUserId)
        {
            // Create a new UserLevel instance with default values
            var userLevel = new UserLevel
            {
                ApplicationUserId = applicationUserId,
                CurrentLevel = 1,
                CurrentExp = 0
            };

            await _unitOfWork.UserLevel.CreateAsync(userLevel);

            return userLevel; 
        }


        public async Task AddExpAsync(string userId, string priority)
        {
            var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);
            if (userLevel != null)
            {
                // Determine experience points based on quest priority
                int expToAdd = GetExpForPriority(priority);

                // Add experience to the user
                userLevel.CurrentExp += expToAdd;

                // Handle level-up logic
                while (userLevel.CurrentExp >= userLevel.ExpForNextLevel)
                {
                    userLevel.CurrentExp -= userLevel.ExpForNextLevel;
                    userLevel.CurrentLevel++;
                }

                await _unitOfWork.SaveAsync();
            }
        }

        public async Task RemoveExpAsync(string userId, string priority)
        {
            var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);

            if (userLevel is null)
            {
                throw new Exception("User level not found."); 
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
