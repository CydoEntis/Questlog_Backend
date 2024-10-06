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
    public class PartyService : IPartyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PartyService> _logger;

        public PartyService(IUnitOfWork unitOfWork, ILogger<PartyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Party> CreateParty(Party party)
        {
            if (party is null)
                throw new ArgumentNullException(nameof(party), "Party id cannot be null");

            try
            {
                var newParty = new Party
                {
                    GuildId = party.GuildId,
                    Name = party.Name,
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
