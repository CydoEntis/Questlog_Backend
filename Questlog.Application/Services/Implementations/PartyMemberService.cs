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
    public class PartyMemberMemberService : IPartyMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PartyMemberMemberService> _logger;

        public PartyMemberMemberService(IUnitOfWork unitOfWork, ILogger<PartyMemberMemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PartyMember> CreatePartyMember(string userId, PartyMember partyMember)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");

            if (partyMember is null)
                throw new ArgumentNullException(nameof(partyMember), "Party Member cannot be null");

            try
            {
                var newPartyMember = new PartyMember
                {
                    //UserId = userId,
                    PartyId = partyMember.PartyId,
                    Role = partyMember.Role
                };
                await _unitOfWork.PartyMember.CreateAsync(newPartyMember);

                return newPartyMember;
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
