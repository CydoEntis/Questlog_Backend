using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
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

    public async Task<ServiceResult<PartyResponseDTO>> GetPartyById(int guildId, int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
        if (!partyIdValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<PartyResponseDTO>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(p => p.Id == partyId && p.GuildId == guildId);

            if (foundParty == null)
            {
                return ServiceResult<PartyResponseDTO>.Failure("Party not found.");
            }

            var partyResponseDTO = _mapper.Map<PartyResponseDTO>(foundParty);

            return ServiceResult<PartyResponseDTO>.Success(partyResponseDTO);
        });
    }

    //public async Task<ServiceResult<List<PartyResponseDTO>>> GetAllParties(int guildId)
    //{
    //    return await HandleExceptions<List<PartyResponseDTO>>(async () =>
    //    {
    //        var parties = await _unitOfWork.Party.GetAllAsync(p => p.GuildId == guildId);

    //        List<PartyResponseDTO> partyResponseDTOs = _mapper.Map<List<PartyResponseDTO>>(parties);

    //        return ServiceResult<List<PartyResponseDTO>>.Success(partyResponseDTOs);
    //    });
    //}

    public async Task<ServiceResult<PartyResponseDTO>> CreateParty(string userId, CreatePartyRequestDTO requestDTO, int guildId)
    {
        var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
        if (!userValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(userValidationResult.ErrorMessage);

        var partyValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Party Request DTO");
        if (!partyValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(partyValidationResult.ErrorMessage);

        return await HandleExceptions<PartyResponseDTO>(async () =>
        {
            var guildMember = await _unitOfWork.GuildMember.GetAsync(gm => gm.UserId == userId && gm.GuildId == guildId);

            var guildMemberValidationResult = ValidationHelper.ValidateObject(guildMember, "Guild Member");
            if (!guildMemberValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(guildMemberValidationResult.ErrorMessage);

            var party = _mapper.Map<Party>(requestDTO);
            party.GuildId = guildId; // Set the guildId here

            Party createdParty = await _unitOfWork.Party.CreateAsync(party);

            var partyMember = new PartyMember
            {
                UserId = userId,
                PartyId = createdParty.Id,
                Role = RoleConstants.Leader,
                JoinedAt = DateTime.UtcNow,
                GuildMemberId = guildMember.Id,
            };

            await _unitOfWork.PartyMember.CreateAsync(partyMember);

            var createdPartyResponseDTO = _mapper.Map<PartyResponseDTO>(createdParty);

            return ServiceResult<PartyResponseDTO>.Success(createdPartyResponseDTO);
        });
    }

    public async Task<ServiceResult<PartyResponseDTO>> UpdateParty(int guildId, UpdatePartyRequestDTO requestDTO)
    {
        var partyValidationResult = ValidationHelper.ValidateObject(requestDTO, "Update Party Request DTO");
        if (!partyValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(partyValidationResult.ErrorMessage);

        var partyIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Party Id");
        if (!partyIdValidationResult.IsSuccess) return ServiceResult<PartyResponseDTO>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<PartyResponseDTO>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(p => p.Id == requestDTO.Id && p.GuildId == guildId);

            if (foundParty == null)
            {
                return ServiceResult<PartyResponseDTO>.Failure("Party not found.");
            }

            var party = _mapper.Map<Party>(requestDTO);
            foundParty.Name = party.Name;
            foundParty.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Party.UpdateAsync(foundParty); // Update the existing party

            var responseDTO = _mapper.Map<PartyResponseDTO>(foundParty);

            return ServiceResult<PartyResponseDTO>.Success(responseDTO);
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
