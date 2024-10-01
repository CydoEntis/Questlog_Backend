using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
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





        private int GetExpForPriority(string priority)
        {
            return priority.ToLower() switch
            {
                "low" => 5,
                "medium" => 10,
                "high" => 15,
                "urgent" => 20,
                _ => 0 // Default case for unknown priority
            };
        }
    }
}
