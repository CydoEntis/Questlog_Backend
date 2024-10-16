using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Queries;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations;

public class PartyService : BaseService, IPartyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;


    public PartyService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<PartyService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;

    }

    public async Task<ServiceResult<GetPartyResponseDto>> GetPartyById(int guildId, int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
        if (!partyIdValidationResult.IsSuccess) return ServiceResult<GetPartyResponseDto>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<GetPartyResponseDto>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(p => p.Id == partyId && p.GuildId == guildId);

            if (foundParty == null)
            {
                return ServiceResult<GetPartyResponseDto>.Failure("Party not found.");
            }

            var partyResponseDTO = _mapper.Map<GetPartyResponseDto>(foundParty);

            return ServiceResult<GetPartyResponseDto>.Success(partyResponseDTO);
        });
    }

    public async Task<ServiceResult<List<GetPartyResponseDto>>> GetAllParties(int guildId, PartyQueryParamsDto queryParams)
    {
        return await HandleExceptions<List<GetPartyResponseDto>>(async () =>
        {
            var options = new QueryOptions<Party>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                IsAscending = queryParams.OrderBy == OrderByOptions.Asc.ToString(),
                IncludeProperties = "PartyMembers,PartyMembers.GuildMember.User",
                DatePropertyName = "CreatedAt",
                Filter = p => p.GuildId == guildId
            };

            options.OrderBy = queryParams.SortBy switch
            {
                "CreatedAt" => query => query.OrderBy(g => g.CreatedAt),
                _ => query => query.OrderBy(g => g.Id)
            };

            var parties = await _unitOfWork.Party.GetAllAsync(options);

            if (parties == null || !parties.Any())
            {
                return ServiceResult<List<GetPartyResponseDto>>.Success(new List<GetPartyResponseDto>());
            }
            
            List<GetPartyResponseDto> guildResponseDTOs = _mapper.Map<List<GetPartyResponseDto>>(parties);

            return ServiceResult<List<GetPartyResponseDto>>.Success(guildResponseDTOs);
            
            
            List<GetPartyResponseDto> partyResponseDTOs = _mapper.Map<List<GetPartyResponseDto>>(parties);

            return ServiceResult<List<GetPartyResponseDto>>.Success(partyResponseDTOs);
        });
    }

    public async Task<ServiceResult<CreatePartyResponseDTO>> CreateParty(string userId, CreatePartyRequestDto requestDto, int guildId)
    {
        // var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
        // if (!userValidationResult.IsSuccess) return ServiceResult<GetPartyResponseDto>.Failure(userValidationResult.ErrorMessage);

        var partyValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Party Request DTO");
        if (!partyValidationResult.IsSuccess) return ServiceResult<CreatePartyResponseDTO>.Failure(partyValidationResult.ErrorMessage);

        return await HandleExceptions<CreatePartyResponseDTO>(async () =>
        {
            // Check if guildMember is a guild leader
            var guildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.UserId == userId && gm.GuildId == guildId && gm.Role == RoleConstants.Leader);
            
            // If the guild member is not a guild leader, then throw error - Must be a guild leader to create a party.
            var guildMemberValidationResult = ValidationHelper.ValidateObject(guildMember, "Guild Member");
            if (!guildMemberValidationResult.IsSuccess) return ServiceResult<CreatePartyResponseDTO>.Failure(guildMemberValidationResult.ErrorMessage);
            
            var party = _mapper.Map<Party>(requestDto);
            party.GuildId = guildId;
            party.PartyLeaderId = userId;
            
            Party createdParty = await _unitOfWork.Party.CreateAsync(party);

            var partyLeader = new PartyMember
            {
                UserId = userId,
                PartyId = createdParty.Id,
                Role = RoleConstants.Leader,
                JoinedOn = DateTime.UtcNow,
                GuildMemberId = guildMember.Id,
                GuildId = guildId
            };
            
            await _unitOfWork.PartyMember.CreateAsync(partyLeader);

            var createdPartyResponseDTO = _mapper.Map<CreatePartyResponseDTO>(createdParty);

            return ServiceResult<CreatePartyResponseDTO>.Success(createdPartyResponseDTO);
        });
    }

    public async Task<ServiceResult<GetPartyResponseDto>> UpdateParty(int guildId, UpdatePartyRequestDTO requestDTO)
    {
        var partyValidationResult = ValidationHelper.ValidateObject(requestDTO, "Update Party Request DTO");
        if (!partyValidationResult.IsSuccess) return ServiceResult<GetPartyResponseDto>.Failure(partyValidationResult.ErrorMessage);

        var partyIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Party Id");
        if (!partyIdValidationResult.IsSuccess) return ServiceResult<GetPartyResponseDto>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<GetPartyResponseDto>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(p => p.Id == requestDTO.Id && p.GuildId == guildId);

            if (foundParty == null)
            {
                return ServiceResult<GetPartyResponseDto>.Failure("Party not found.");
            }

            var party = _mapper.Map<Party>(requestDTO);
            foundParty.Name = party.Name;
            foundParty.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Party.UpdateAsync(foundParty); // Update the existing party

            var responseDTO = _mapper.Map<GetPartyResponseDto>(foundParty);

            return ServiceResult<GetPartyResponseDto>.Success(responseDTO);
        });
    }

    public async Task<ServiceResult<int>> DeleteParty(int guildId, int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
        if (!partyIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(p => p.Id == partyId && p.GuildId == guildId);

            if (foundParty == null)
            {
                return ServiceResult<int>.Failure("Party not found.");
            }

            // Delete the party
            await _unitOfWork.Party.RemoveAsync(foundParty);

            return ServiceResult<int>.Success(foundParty.Id);
        });
    }
}
