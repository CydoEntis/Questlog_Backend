using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class PartyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PartyService> _logger;

        public PartyService(IUnitOfWork unitOfWork, ILogger<PartyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Party> CreateParty(string userId, Party guild)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");

            if (guild is null)
                throw new ArgumentNullException(nameof(guild), "Party cannot be null");

            try
            {
                var newParty = new Party
                {
                    //DisplayName = character.DisplayName,
                    //Archetype = character.Archetype,
                };
                await _unitOfWork.Party.CreateAsync(newParty);

                return newParty;
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


    }
}
