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
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class GuildMemberService : BaseService, IGuildMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GuildMemberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<GuildMemberService> logger, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult<GuildMemberResponseDTO>> GetGuildMember(int guildId, string userId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<GuildMemberResponseDTO>.Failure(idValidation.ErrorMessage);

            var userIdValidation = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userIdValidation.IsSuccess) return ServiceResult<GuildMemberResponseDTO>.Failure(userIdValidation.ErrorMessage);

            return await HandleExceptions<GuildMemberResponseDTO>(async () =>
            {
                GuildMember foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.GuildId == guildId && gm.UserId == userId);

                if (foundGuildMember == null)
                {
                    return ServiceResult<GuildMemberResponseDTO>.Failure("Guild member not found.");
                }

                var guildMemberResponseDTO = _mapper.Map<GuildMemberResponseDTO>(foundGuildMember);

                return ServiceResult<GuildMemberResponseDTO>.Success(guildMemberResponseDTO);
            });
        }

        public async Task<ServiceResult<List<GuildMemberResponseDTO>>> GetAllGuildMembers(int guildId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<List<GuildMemberResponseDTO>>.Failure(idValidation.ErrorMessage);

            return await HandleExceptions<List<GuildMemberResponseDTO>>(async () =>
            {
                List<GuildMember> guildMembers = await _unitOfWork.GuildMember.GetAllAsync(gm => gm.GuildId == guildId);

                List<GuildMemberResponseDTO> guildMemberResponseDTOs = _mapper.Map<List<GuildMemberResponseDTO>>(guildMembers);

                return ServiceResult<List<GuildMemberResponseDTO>>.Success(guildMemberResponseDTOs);
            });
        }

        public async Task<ServiceResult<GuildMemberResponseDTO>> CreateGuildMember(CreateGuildMemberRequestDTO requestDTO)
        {
            if (requestDTO == null) ServiceResult<GuildMemberResponseDTO>.Failure("Must provide a valid Guild Member");

            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(requestDTO.UserId, _userManager);
            if (!userValidationResult.IsSuccess) return ServiceResult<GuildMemberResponseDTO>.Failure(userValidationResult.ErrorMessage);

            return await HandleExceptions<GuildMemberResponseDTO>(async () =>
            {
                var guildMember = _mapper.Map<GuildMember>(requestDTO);

                var createdGuildMember = await _unitOfWork.GuildMember.CreateAsync(guildMember);

                var responseDTO = _mapper.Map<GuildMemberResponseDTO>(createdGuildMember);

                return ServiceResult<GuildMemberResponseDTO>.Success(responseDTO);
            });
        }


        public async Task<ServiceResult<GuildMemberResponseDTO>> UpdateGuildMember(UpdateGuildMemberRequestDTO requestDTO)
        {
            var guildValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Guild Request DTO");
            if (!guildValidationResult.IsSuccess) return ServiceResult<GuildMemberResponseDTO>.Failure(guildValidationResult.ErrorMessage);

            var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<GuildMemberResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<GuildMemberResponseDTO>(async () =>
            {
                GuildMember foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == requestDTO.Id);

                if (foundGuildMember == null)
                    return ServiceResult<GuildMemberResponseDTO>.Failure("Guild Member not found");

                var guildMember = _mapper.Map<GuildMember>(requestDTO);

                foundGuildMember.Role = guildMember.Role;
                foundGuildMember.UpdatedOn = DateTime.UtcNow;

                await _unitOfWork.GuildMember.UpdateAsync(foundGuildMember);

                var guildMemberResponseDTO = _mapper.Map<GuildMemberResponseDTO>(foundGuildMember);

                return ServiceResult<GuildMemberResponseDTO>.Success(guildMemberResponseDTO);
            });
        }

        public async Task<ServiceResult<int>> RemoveGuildMember(int guildId)
        {
            var guildIdValidationResult = ValidationHelper.ValidateId(guildId, "Guild Id");
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(guildIdValidationResult.ErrorMessage);

            return await HandleExceptions<int>(async () =>
            {
                var foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == guildId);
                if (foundGuildMember == null)
                    return ServiceResult<int>.Failure("Guild Member not found");

                await _unitOfWork.GuildMember.RemoveAsync(foundGuildMember);

                return ServiceResult<int>.Success(foundGuildMember.Id);
            });
        }

    }
}
