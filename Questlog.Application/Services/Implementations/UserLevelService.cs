using Questlog.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class UserLevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserLevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddExpAsync(string userId, int expToAdd)
        {
            var userLevel = await _unitOfWork.UserLevel.GetUserLevelByUserIdAsync(userId);
            if (userLevel != null)
            {
                userLevel.CurrentExp += expToAdd;

                // If the user's experience exceeds the required experience for the next level, level up
                while (userLevel.CurrentExp >= userLevel.ExpForNextLevel)
                {
                    userLevel.CurrentExp -= userLevel.ExpForNextLevel;
                    userLevel.CurrentLevel++;
                }

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
