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
    public class GuildMemberService : IGuildMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GuildMemberService> _logger;

        public GuildMemberService(IUnitOfWork unitOfWork, ILogger<GuildMemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GuildMember> CreateGuildMember(GuildMember guildMember)
        {
            if (guildMember is null)
                throw new ArgumentNullException(nameof(guildMember), "Guild Member cannot be null");

            if (string.IsNullOrEmpty(guildMember.UserId))
                throw new ArgumentNullException(nameof(guildMember.UserId), "User id cannot be null");

            try
            {
                GuildMember createdGuildMember = await _unitOfWork.GuildMember.CreateAsync(guildMember);

                return createdGuildMember;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating Guild Member.");
                throw new Exception("An error occurred while saving to the database. Please try again.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating Guild Member.");
                throw;
            }
        }
    }
}
