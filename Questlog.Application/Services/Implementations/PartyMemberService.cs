using Microsoft.AspNetCore.Identity;
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
    public class PartyMemberService : IPartyMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PartyMemberService> _logger;

        public PartyMemberService(IUnitOfWork unitOfWork, ILogger<PartyMemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PartyMember> CreatePartyMember(PartyMember partyMember)
        {
            if (partyMember is null)
                throw new ArgumentNullException(nameof(partyMember), "Party Member cannot be null");

            try
            {
                PartyMember createdPartyMember = await _unitOfWork.PartyMember.CreateAsync(partyMember);

                return createdPartyMember;
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
