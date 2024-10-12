using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.DTOs.GuildMember.Response;
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

        public async Task<ServiceResult<GetGuildResponseDTO>> GetGuildById(int guildId)
        {
            var guildIdValidationResult = ValidationHelper.ValidateId(guildId, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<GetGuildResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<GetGuildResponseDTO>(async () =>
            {
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId, includeProperties: "GuildMembers,Parties");

                if (foundGuild == null)
                {
                    return ServiceResult<GetGuildResponseDTO>.Failure("Guild not found.");
                }

                var guildResponseDTO = _mapper.Map<GetGuildResponseDTO>(foundGuild);

                return ServiceResult<GetGuildResponseDTO>.Success(guildResponseDTO);
            });
        }

        public async Task<ServiceResult<List<GetAllGuildsResponseDTO>>> GetAllGuilds()
        {
            return await HandleExceptions<List<GetAllGuildsResponseDTO>>(async () =>
            {
                var guilds = await _unitOfWork.Guild.GetAllAsync(orderBy: q => q.OrderBy(g => g.CreatedAt), ascending: false);

                List<GetAllGuildsResponseDTO> guildResponseDTOs = _mapper.Map<List<GetAllGuildsResponseDTO>>(guilds);

                return ServiceResult<List<GetAllGuildsResponseDTO>>.Success(guildResponseDTOs);
            });
        }

        public async Task<ServiceResult<CreateGuildResponseDTO>> CreateGuild(string userId, CreateGuildRequestDTO requestDTO)
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess) return ServiceResult<CreateGuildResponseDTO>.Failure(userValidationResult.ErrorMessage);

            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<CreateGuildResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            return await HandleExceptions<CreateGuildResponseDTO>(async () =>
            {
                // Use AutoMapper to map the request DTO to the Guild entity
                var guild = _mapper.Map<Guild>(requestDTO);
                guild.GuildLeaderId = userId; // Set additional properties not in the DTO

                // Save the new guild
                Guild createdGuild = await _unitOfWork.Guild.CreateAsync(guild);

                // Create and save the guild leader
                var guildLeader = new GuildMember
                {
                    UserId = userId,
                    GuildId = createdGuild.Id,
                    Role = RoleConstants.GuildLeader,
                    JoinedOn = DateTime.UtcNow,
                };

                var createdGuildLeader = await _unitOfWork.GuildMember.CreateAsync(guildLeader);

                // Set the GuildLeader on the created guild entity (or you can fetch it)
                createdGuild.GuildLeader = createdGuildLeader;

                // Use AutoMapper to map the created guild (including the guild leader) to the response DTO
                var createGuildResponseDTO = _mapper.Map<CreateGuildResponseDTO>(createdGuild);

                return ServiceResult<CreateGuildResponseDTO>.Success(createGuildResponseDTO);
            });
        }


        public async Task<ServiceResult<UpdateGuildDetailsResponseDTO>> UpdateGuildDetails(UpdateGuildDetailsRequestDTO requestDTO, string userId)
        {
            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Update Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<UpdateGuildDetailsResponseDTO>(async () =>
            {
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == requestDTO.Id && g.GuildLeaderId == userId);

                if (foundGuild == null)
                {
                    return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure("Guild not found.");
                }

                if (!await IsUserGuildLeader(requestDTO.Id, userId))
                {
                    return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure("User is not authorized to update the guild leader.");
                }

                foundGuild.Name = requestDTO.Name.Trim();
                foundGuild.Description = requestDTO.Description.Trim();
                foundGuild.UpdatedAt = DateTime.UtcNow;
                foundGuild.Color = requestDTO.Color;

                await _unitOfWork.Guild.UpdateAsync(foundGuild);

                var responseDTO = _mapper.Map<UpdateGuildDetailsResponseDTO>(foundGuild);

                return ServiceResult<UpdateGuildDetailsResponseDTO>.Success(responseDTO);
            });
        }

        public async Task<ServiceResult<UpdateGuildLeaderResponseDTO>> UpdateGuildLeader(UpdateGuildLeaderRequestDTO requestDTO, string userId)
        {
            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Update Guild Request DTO");
            if (!guildValidationResult.IsSuccess)
                return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
            if (!guildIdValidationResult.IsSuccess)
                return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<UpdateGuildLeaderResponseDTO>(async () =>
            {
                if (!await IsUserGuildLeader(requestDTO.Id, userId))
                {
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("User is not authorized to update the guild leader.");
                }

                // Find the guild
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == requestDTO.Id);
                if (foundGuild == null)
                {
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Guild not found.");
                }

                // Find and demote the current guild leader
                var currentGuildLeader = await _unitOfWork.GuildMember.GetAsync(g => g.UserId == foundGuild.GuildLeaderId);
                if (currentGuildLeader is null)
                {
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Current guild leader not found.");
                }
                currentGuildLeader.Role = RoleConstants.GuildMember;

                // Promote the new guild leader
                var guildMemberToBecomeGuildLeader = await _unitOfWork.GuildMember.GetAsync(g => g.UserId == requestDTO.GuildLeaderId);
                if (guildMemberToBecomeGuildLeader is null)
                {
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Future guild leader not found.");
                }
                guildMemberToBecomeGuildLeader.Role = RoleConstants.GuildLeader;

                // Update the guild leader in the guild entity
                foundGuild.GuildLeaderId = requestDTO.GuildLeaderId;
                await _unitOfWork.Guild.UpdateAsync(foundGuild);

                // Create the response DTO with both leaders
                var responseDTO = new UpdateGuildLeaderResponseDTO
                {
                    PreviousGuildLeader = _mapper.Map<GuildMemberResponseDTO>(currentGuildLeader),
                    NewGuildLeader = _mapper.Map<GuildMemberResponseDTO>(guildMemberToBecomeGuildLeader),
                    GuildId = foundGuild.Id
                };

                return ServiceResult<UpdateGuildLeaderResponseDTO>.Success(responseDTO);
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

        private async Task<bool> IsUserGuildLeader(int guildId, string userId)
        {
            var guild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId);
            return guild?.GuildLeaderId == userId; 
        }
    }


}
