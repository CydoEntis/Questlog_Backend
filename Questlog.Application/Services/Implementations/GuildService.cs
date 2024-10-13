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
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId, includeProperties: "GuildMembers,GuildMembers.User,Parties");

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
                var guilds = await _unitOfWork.Guild.GetAllAsync(orderBy: q => q.OrderBy(g => g.CreatedAt), ascending: false, includeProperties: "GuildMembers,GuildMembers.User");

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
                var guild = _mapper.Map<Guild>(requestDTO);
                guild.GuildLeaderId = userId;

                Guild createdGuild = await _unitOfWork.Guild.CreateAsync(guild);

                var guildLeader = new GuildMember
                {
                    UserId = userId,
                    GuildId = createdGuild.Id,
                    Role = RoleConstants.GuildLeader,
                    JoinedOn = DateTime.UtcNow,
                };

                await _unitOfWork.GuildMember.CreateAsync(guildLeader);

                var guildWithLeader = await _unitOfWork.Guild
                           .GetAsync(g => g.Id == guild.Id, includeProperties: "GuildMembers,GuildMembers.User");

                var createGuildResponseDTO = _mapper.Map<CreateGuildResponseDTO>(guildWithLeader);

                return ServiceResult<CreateGuildResponseDTO>.Success(createGuildResponseDTO);
            });
        }



        public async Task<ServiceResult<UpdateGuildDetailsResponseDTO>> UpdateGuildDetails(UpdateGuildDetailsRequestDTO requestDTO, string userId)
        {
            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Update Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            if (!await IsUserGuildLeader(requestDTO.Id, userId))
            {
                return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure("User is not authorized to update the guild leader.");
            }

            return await HandleExceptions<UpdateGuildDetailsResponseDTO>(async () =>
            {
                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == requestDTO.Id && g.GuildLeaderId == userId);

                if (foundGuild == null)
                {
                    return ServiceResult<UpdateGuildDetailsResponseDTO>.Failure("Guild not found.");
                }



                foundGuild.Name = requestDTO.Name.Trim();
                foundGuild.Description = requestDTO.Description.Trim();
                foundGuild.Color = requestDTO.Color;
                foundGuild.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Guild.UpdateAsync(foundGuild);

                var responseDTO = _mapper.Map<UpdateGuildDetailsResponseDTO>(foundGuild);

                return ServiceResult<UpdateGuildDetailsResponseDTO>.Success(responseDTO);
            });
        }

        public async Task<ServiceResult<UpdateGuildLeaderResponseDTO>> UpdateGuildLeader(int guildId, string userId, UpdateGuildLeaderRequestDTO requestDTO)
        {
            var validations = new[]
            {
                ValidationHelper.ValidateId(guildId, "Guild Id"),
                ValidationHelper.ValidateId(requestDTO.GuildLeaderId, "Guild leader id")
            };

            var failedValidation = validations.FirstOrDefault(v => !v.IsSuccess);
            if (failedValidation != null)
                return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure(failedValidation.ErrorMessage);

            if (guildId != requestDTO.GuildId)
                return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Guild member must be from same guild");

            if (!await IsUserGuildLeader(guildId, userId))
                return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("User is not authorized to update the guild leader.");

            return await HandleExceptions<UpdateGuildLeaderResponseDTO>(async () =>
            {
                var newGuildLeader = await _unitOfWork.GuildMember.GetAsync(gm => gm.UserId == requestDTO.GuildLeaderId, includeProperties: "User");
                if (newGuildLeader is null)
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("New guild leader does not exist");

                var foundGuild = await _unitOfWork.Guild.GetAsync(g => g.Id == guildId);
                if (foundGuild is null)
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Guild not found");

                var oldGuildLeader = await _unitOfWork.GuildMember.GetAsync(gm => gm.UserId == foundGuild.GuildLeaderId, includeProperties: "User");
                if (oldGuildLeader is null)
                    return ServiceResult<UpdateGuildLeaderResponseDTO>.Failure("Old guild leader not found");

                oldGuildLeader.Role = RoleConstants.GuildMember;
                newGuildLeader.Role = RoleConstants.GuildLeader;

                await _unitOfWork.GuildMember.UpdateAsync(oldGuildLeader);
                await _unitOfWork.GuildMember.UpdateAsync(newGuildLeader);

                foundGuild.GuildLeaderId = newGuildLeader.UserId;
                await _unitOfWork.Guild.UpdateAsync(foundGuild);

                var responseDTO = new UpdateGuildLeaderResponseDTO
                {
                    OldGuildLeader = _mapper.Map<GetGuildMemberResponseDTO>(oldGuildLeader),
                    NewGuildLeader = _mapper.Map<GetGuildMemberResponseDTO>(newGuildLeader),
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
