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

namespace Questlog.Application.Services.Implementations
{
    public class MainQuestService : IMainQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MainQuestService> _logger; // Inject logger

        public MainQuestService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MainQuestService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<MainQuest> GetMainQuest(int mainQuestId)
        {
            try
            {
                var mainQuest = await _unitOfWork.MainQuest.GetAsync(mainQuest => mainQuest.Id == mainQuestId, includeProperties: "QuestBoards");
                if (mainQuest == null)
                {
                    _logger.LogWarning($"MainQuest with ID {mainQuestId} not found.");
                    throw new KeyNotFoundException($"Could not find Main Quest with matching Id");
                }
                return mainQuest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving MainQuest with ID {mainQuestId}.");
                throw; 
            }
        }

        public async Task<int> CreateMainQuest(MainQuest mainQuest)
        {
            if (mainQuest == null)
            {
                throw new ArgumentNullException(nameof(mainQuest), "MainQuest cannot be null.");
            }

            try
            {
                var newMainQuest = await _unitOfWork.MainQuest.CreateAsync(mainQuest);

                if (mainQuest.QuestBoards != null)
                {
                    foreach (var questBoard in mainQuest.QuestBoards)
                    {
                        questBoard.MainQuestId = newMainQuest.Id; // Set the foreign key
                    }

                    await _unitOfWork.SaveAsync();
                }

                return newMainQuest.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating MainQuest.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating MainQuest.");
                throw; 
            }
        }

        public async Task<>
    }
}
