using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class GuildService : BaseService, IGuildService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public GuildService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<GuildService> logger, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<ServiceResult<GuildResponseDTO>> GetGuildById(int guildId)
        {
            var guildIdValidationResult = ValidationHelper.ValidateId(guildId, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<GuildResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<GuildResponseDTO>(async () =>
            {
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId);

                if (foundGuild == null)
                {
                    return ServiceResult<GuildResponseDTO>.Failure("Guild not found.");
                }

                var guildResponseDTO = _mapper.Map<GuildResponseDTO>(foundGuild);

                return ServiceResult<GuildResponseDTO>.Success(guildResponseDTO);
            });
        }

        public async Task<ServiceResult<List<GuildResponseDTO>>> GetAllGuilds()
        {
            return await HandleExceptions<List<GuildResponseDTO>>(async () =>
            {
                var guilds = await _unitOfWork.Guild.GetAllAsync();

                List<GuildResponseDTO> guildResponseDTOs = _mapper.Map<List<GuildResponseDTO>>(guilds);

                return ServiceResult<List<GuildResponseDTO>>.Success(guildResponseDTOs);
            });
        }

        public async Task<ServiceResult<GuildResponseDTO>> CreateGuild(string userId, CreateGuildRequestDTO requestDTO)
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess) return ServiceResult<GuildResponseDTO>.Failure(userValidationResult.ErrorMessage);

            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<GuildResponseDTO>.Failure(guildValidationResult.ErrorMessage);


            return await HandleExceptions<GuildResponseDTO>(async () =>
            {
                var guild = _mapper.Map<Guild>(requestDTO);
                var newGuild = new Guild
                {
                    Name = guild.Name,
                    Description = guild.Description,
                    GuildLeaderId = userId
                };

                Guild createdGuild = await _unitOfWork.Guild.CreateAsync(newGuild);

                var guildMember = new GuildMember
                {
                    UserId = userId,
                    GuildId = createdGuild.Id,
                    Role = RoleConstants.GuildLeader,
                    JoinedOn = DateTime.UtcNow,
                };

                await _unitOfWork.GuildMember.CreateAsync(guildMember);

                var createdGuildResponseDTO = _mapper.Map<GuildResponseDTO>(createdGuild);

                return ServiceResult<GuildResponseDTO>.Success(createdGuildResponseDTO);
            });
        }

        public async Task<ServiceResult<GuildResponseDTO>> UpdateGuild(UpdateGuildRequestDTO requestDTO)
        {
            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<GuildResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<GuildResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<GuildResponseDTO>(async () =>
            {

                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == requestDTO.Id);

                if (foundGuild == null)
                {
                    return ServiceResult<GuildResponseDTO>.Failure("Guild not found.");
                }

                var guild = _mapper.Map<Guild>(requestDTO);

                foundGuild.Name = guild.Name;
                foundGuild.Description = guild.Description;
                foundGuild.UpdatedAt = DateTime.UtcNow;
                foundGuild.GuildLeaderId = guild.GuildLeaderId;

                await _unitOfWork.Guild.UpdateAsync(guild);

                var responseDTO = _mapper.Map<GuildResponseDTO>(foundGuild);

                return ServiceResult<GuildResponseDTO>.Success(responseDTO);
            });
        }

        public async Task<ServiceResult<int>> DeleteGuild(int guildId)
        {
            var guildIdValidationResult = ValidationHelper.ValidateId(guildId, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<int>(async () =>
            {
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId);

                if (foundGuild == null)
                {
                    return ServiceResult<int>.Failure("Guild not found.");
                }

                // Delete the guild
                await _unitOfWork.Guild.RemoveAsync(foundGuild);

                return ServiceResult<int>.Success(foundGuild.Id);
            });
        }
    }
}
