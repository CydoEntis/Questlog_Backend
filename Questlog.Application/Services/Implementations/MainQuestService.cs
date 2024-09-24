using AutoMapper;
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
    public class MainQuestService : IMainQuestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainQuestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<MainQuest> GetMainQuest(int mainQuestId)
        {
            var mainQuest = await _unitOfWork.MainQuest.GetAsync(mainQuest => mainQuest.Id == mainQuestId, includeProperties: "QuestBoards");
            return mainQuest;
        }

        public async Task<int> CreateMainQuest(MainQuest mainQuest)
        {
            if (mainQuest == null)
            {
                throw new ArgumentNullException(nameof(mainQuest), "MainQuest cannot be null.");
            }

            try
            {
                // First, create and save the MainQuest to get the ID
                var newMainQuest = await _unitOfWork.MainQuest.CreateAsync(mainQuest);

                // Ensure QuestBoards are loaded and attached correctly
                if (mainQuest.QuestBoards != null)
                {
                    // Update the QuestBoards with the generated MainQuest ID
                    foreach (var questBoard in mainQuest.QuestBoards)
                    {
                        questBoard.MainQuestId = newMainQuest.Id; // Set the foreign key
                    }

                    // Save changes after setting the MainQuestId
                    await _unitOfWork.SaveAsync();
                }

                return newMainQuest.Id;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating MainQuest: {ex.Message}");
                throw;
            }
        }



    }
}
