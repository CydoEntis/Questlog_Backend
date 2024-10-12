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
    public class CharacterService : ICharacterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(IUnitOfWork unitOfWork, ILogger<CharacterService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Character> GetCharacterAsync(string userId)
        {
            try
            {
                var character = await _unitOfWork.Character.GetCharacterAsync(userId);

                if (character is null)
                {
                    _logger.LogWarning($"Character not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find Character for User with id {userId}");
                }

                return character;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving Character for user with id: {userId}");
                throw;
            }

        }

        public async Task<Character> CreateCharacterAsync(string userId, Character character)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");
            }

            if (character is null)
            {
                throw new ArgumentNullException(nameof(character), "Character cannot be null");
            }

            try
            {
                var newCharacter = new Character {
                    DisplayName = character.DisplayName,
                    Avatar = character.Avatar,
                };

                await _unitOfWork.Character.CreateAsync(character);

                return character;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating Character.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating Character.");
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
                var character = await _unitOfWork.Character.GetCharacterAsync(userId);
                if (character is null)
                {
                    _logger.LogWarning($"Character not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find Character for User with id {userId}");
                }
                int expToAdd = GetExpForPriority(priority);

                character.CurrentExp += expToAdd;

                while (character.CurrentExp >= character.ExpToNextLevel)
                {
                    character.CurrentExp -= character.ExpToNextLevel;
                    character.CurrentLevel++;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding exp to Character.");
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
                var character = await _unitOfWork.Character.GetCharacterAsync(userId);
                if (character is null)
                {
                    _logger.LogWarning($"Character not found for user with ID {userId}");
                    throw new KeyNotFoundException($"Could not find Character for User with id {userId}");
                }
                int expToDeduct = GetExpForPriority(priority);

                int newExp = character.CurrentExp - expToDeduct;

                while (newExp < 0 && character.CurrentLevel > 1)
                {
                    character.CurrentLevel--;
                    newExp += character.CalculateExpForLevel();
                }

                character.CurrentExp = Math.Max(0, newExp);

                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while removing exp from Character.");
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
