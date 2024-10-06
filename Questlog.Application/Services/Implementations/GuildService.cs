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
    public class GuildService : IGuildService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GuildService> _logger;


        public GuildService(IUnitOfWork unitOfWork, ILogger<GuildService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public async Task<Guild> CreateGuild(string userId, Guild guild)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User id cannot be null");

            if (guild is null)
                throw new ArgumentNullException(nameof(guild), "Guild cannot be null");

            try
            {
                var newGuild = new Guild
                {
                    Name = guild.Name,
                    Description = guild.Description,
                    GuildLeaderId = userId
                };

                Guild createdGuild = await _unitOfWork.Guild.CreateAsync(newGuild);

                return createdGuild;
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
