using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation; // Add this namespace
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class GuildMemberService : BaseService, IGuildMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GuildMemberService> _logger;

        public GuildMemberService(IUnitOfWork unitOfWork, ILogger<GuildMemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResult<GuildMember>> GetGuildMember(int guildId, string userId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<GuildMember>.Failure(idValidation.ErrorMessage);

            var userIdValidation = ValidationHelper.ValidateUserId(userId);
            if (!userIdValidation.IsSuccess) return ServiceResult<GuildMember>.Failure(userIdValidation.ErrorMessage);

            return await HandleExceptions<GuildMember>(async () =>
            {
                GuildMember foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.GuildId == guildId && gm.UserId == userId);
                return ServiceResult<GuildMember>.Success(foundGuildMember);
            });
        }

        public async Task<ServiceResult<List<GuildMember>>> GetAllGuildMembers(int guildId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<List<GuildMember>>.Failure(idValidation.ErrorMessage);

            return await HandleExceptions<List<GuildMember>>(async () =>
            {
                List<GuildMember> guildMembers = await _unitOfWork.GuildMember.GetAllAsync(gm => gm.GuildId == guildId);

                if (guildMembers.Count == 0)
                    return ServiceResult<List<GuildMember>>.Failure("No guild members found for the specified guild.");

                return ServiceResult<List<GuildMember>>.Success(guildMembers);
            });
        }

        public async Task<ServiceResult<GuildMember>> CreateGuildMember(GuildMember guildMember)
        {
            var validationResult = ValidateGuildMember(guildMember);
            if (!validationResult.IsSuccess) return ServiceResult<GuildMember>.Failure(validationResult.ErrorMessage);

            return await HandleExceptions<GuildMember>(async () =>
            {
                var createdGuildMember = await _unitOfWork.GuildMember.CreateAsync(guildMember);
                return ServiceResult<GuildMember>.Success(createdGuildMember);
            });
        }

        public async Task<ServiceResult<GuildMember>> UpdateGuildMember(GuildMember guildMember)
        {
            var validationResult = ValidateGuildMember(guildMember);
            if (!validationResult.IsSuccess) return ServiceResult<GuildMember>.Failure(validationResult.ErrorMessage);

            return await HandleExceptions<GuildMember>(async () =>
            {
                GuildMember foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == guildMember.Id);
                if (foundGuildMember == null)
                    return ServiceResult<GuildMember>.Failure("Guild Member not found");

                foundGuildMember.Role = guildMember.Role;
                foundGuildMember.UpdatedOn = DateTime.UtcNow;

                await _unitOfWork.GuildMember.UpdateAsync(foundGuildMember);

                return ServiceResult<GuildMember>.Success(foundGuildMember);
            });
        }

        public async Task<ServiceResult<GuildMember>> RemoveGuildMember(GuildMember guildMember)
        {
            var validationResult = ValidateGuildMember(guildMember);
            if (!validationResult.IsSuccess) return ServiceResult<GuildMember>.Failure(validationResult.ErrorMessage);

            return await HandleExceptions<GuildMember>(async () =>
            {
                var foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == guildMember.Id);
                if (foundGuildMember == null)
                    return ServiceResult<GuildMember>.Failure("Guild Member not found");

                await _unitOfWork.GuildMember.RemoveAsync(foundGuildMember);

                return ServiceResult<GuildMember>.Success(foundGuildMember);
            });
        }

        private ServiceResult ValidateGuildMember(GuildMember guildMember)
        {
            if (guildMember == null)
                return ServiceResult.Failure("Must provide a valid Guild Member");

            var userIdValidation = ValidationHelper.ValidateUserId(guildMember.UserId);
            if (!userIdValidation.IsSuccess) return userIdValidation;

            return ServiceResult.Success();
        }
    }
}
