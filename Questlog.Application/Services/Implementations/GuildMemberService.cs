using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Response;
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

        public async Task<ServiceResult<GetGuildMemberResponseDTO>> GetGuildMember(int guildId, int guildMemberId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<GetGuildMemberResponseDTO>.Failure(idValidation.ErrorMessage);

            var memberIdValidation = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
            if (!memberIdValidation.IsSuccess) return ServiceResult<GetGuildMemberResponseDTO>.Failure(memberIdValidation.ErrorMessage);

            return await HandleExceptions<GetGuildMemberResponseDTO>(async () =>
            {
                GuildMember foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.GuildId == guildId && gm.Id == guildMemberId);

                if (foundGuildMember == null)
                {
                    return ServiceResult<GetGuildMemberResponseDTO>.Failure("Guild member not found.");
                }

                var guildMemberResponseDTO = _mapper.Map<GetGuildMemberResponseDTO>(foundGuildMember);

                return ServiceResult<GetGuildMemberResponseDTO>.Success(guildMemberResponseDTO);
            });
        }


        public async Task<ServiceResult<List<GetGuildMemberResponseDTO>>> GetAllGuildMembers(int guildId)
        {
            var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!idValidation.IsSuccess) return ServiceResult<List<GetGuildMemberResponseDTO>>.Failure(idValidation.ErrorMessage);

            return await HandleExceptions<List<GetGuildMemberResponseDTO>>(async () =>
            {
                List<GuildMember> guildMembers = await _unitOfWork.GuildMember.GetAllAsync(gm => gm.GuildId == guildId, includeProperties: "User");

                List<GetGuildMemberResponseDTO> guildMemberResponseDTOs = _mapper.Map<List<GetGuildMemberResponseDTO>>(guildMembers);

                return ServiceResult<List<GetGuildMemberResponseDTO>>.Success(guildMemberResponseDTOs);
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
                guildMember.Role = RoleConstants.Member;

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

        public async Task<ServiceResult<int>> RemoveGuildMember(int guildId, int guildMemberId)
        {
            var guildIdValidationResult = ValidationHelper.ValidateId(guildId, nameof(guildId));
            if (!guildIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(guildIdValidationResult.ErrorMessage);

            var memberIdValidationResult = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
            if (!memberIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(memberIdValidationResult.ErrorMessage);

            return await HandleExceptions<int>(async () =>
            {
                var foundGuildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == guildMemberId && gm.GuildId == guildId);
                if (foundGuildMember == null)
                    return ServiceResult<int>.Failure("Guild Member not found");

                await _unitOfWork.GuildMember.RemoveAsync(foundGuildMember);

                return ServiceResult<int>.Success(foundGuildMember.Id);
            });
        }


    }
}
