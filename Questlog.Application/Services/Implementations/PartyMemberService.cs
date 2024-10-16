using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations;


public class PartyMemberService : BaseService, IPartyMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public PartyMemberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<PartyMemberService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PartyMemberResponseDTO>> GetPartyMember(int partyMemberId)
    {
        var idValidation = ValidationHelper.ValidateId(partyMemberId, nameof(partyMemberId));
        if (!idValidation.IsSuccess) return ServiceResult<PartyMemberResponseDTO>.Failure(idValidation.ErrorMessage);



        return await HandleExceptions<PartyMemberResponseDTO>(async () =>
        {
            PartyMember foundPartyMember = await _unitOfWork.PartyMember.GetAsync(pm => pm.Id == partyMemberId);

            if (foundPartyMember == null)
                return ServiceResult<PartyMemberResponseDTO>.Failure("Party member not found.");

            var partyMemberMemberResponseDTO = _mapper.Map<PartyMemberResponseDTO>(foundPartyMember);

            return ServiceResult<PartyMemberResponseDTO>.Success(partyMemberMemberResponseDTO);
        });
    }

    //public async Task<ServiceResult<List<PartyMemberResponseDTO>>> GetAllPartyMembers(int partyId)
    //{
    //    var idValidation = ValidationHelper.ValidateId(partyId, "Party Id");
    //    if (!idValidation.IsSuccess) return ServiceResult<List<PartyMemberResponseDTO>>.Failure(idValidation.ErrorMessage);

    //    return await HandleExceptions<List<PartyMemberResponseDTO>>(async () =>
    //    {
    //        List<PartyMember> partiesPartyMembers = await _unitOfWork.PartyMember.GetAllAsync(pm => pm.PartyId == partyId);

    //        List<PartyMemberResponseDTO> partyMemberResponseDTOs = _mapper.Map<List<PartyMemberResponseDTO>>(partiesPartyMembers);

    //        return ServiceResult<List<PartyMemberResponseDTO>>.Success(partyMemberResponseDTOs);
    //    });
    //}

    public async Task<ServiceResult<PartyMemberResponseDTO>> CreatePartyMember(CreatePartyMemberRequestDto requestDTO)
    {
        if (requestDTO == null) ServiceResult<PartyMemberResponseDTO>.Failure("Must provide a valid Party Member");



        return await HandleExceptions<PartyMemberResponseDTO>(async () =>
        {
            var guildMember = _unitOfWork.GuildMember.GetAsync(g => g.GuildId == requestDTO.GuildId);

            var guildMemberValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Party Request DTO");
            if (!guildMemberValidationResult.IsSuccess) return ServiceResult<PartyMemberResponseDTO>.Failure(guildMemberValidationResult.ErrorMessage);

            var partyMemberMember = _mapper.Map<PartyMember>(requestDTO);

            var createdPartyMember = await _unitOfWork.PartyMember.CreateAsync(partyMemberMember);

            var responseDTO = _mapper.Map<PartyMemberResponseDTO>(createdPartyMember);

            return ServiceResult<PartyMemberResponseDTO>.Success(responseDTO);
        });
    }

    public async Task<ServiceResult<PartyMemberResponseDTO>> UpdatePartyMember(UpdatePartyMemberRequestDTO requestDTO)
    {
        var partyMemberValidationResult = ValidationHelper.ValidateObject(requestDTO, "Create Party Request DTO");
        if (!partyMemberValidationResult.IsSuccess) return ServiceResult<PartyMemberResponseDTO>.Failure(partyMemberValidationResult.ErrorMessage);

        var partyMemberIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Party Id");
        if (!partyMemberIdValidationResult.IsSuccess) return ServiceResult<PartyMemberResponseDTO>.Failure(partyMemberIdValidationResult.ErrorMessage);

        return await HandleExceptions<PartyMemberResponseDTO>(async () =>
        {
            PartyMember foundPartyMember = await _unitOfWork.PartyMember.GetAsync(pm => pm.Id == requestDTO.Id);

            if (foundPartyMember == null)
                return ServiceResult<PartyMemberResponseDTO>.Failure("Party Member not found");

            var partyMemberMember = _mapper.Map<PartyMember>(requestDTO);

            foundPartyMember.Role = partyMemberMember.Role;
            foundPartyMember.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PartyMember.UpdateAsync(foundPartyMember);

            var partyMemberMemberResponseDTO = _mapper.Map<PartyMemberResponseDTO>(foundPartyMember);

            return ServiceResult<PartyMemberResponseDTO>.Success(partyMemberMemberResponseDTO);
        });
    }

    public async Task<ServiceResult<int>> RemovePartyMember(int partyMemberId)
    {
        var partyMemberIdValidationResult = ValidationHelper.ValidateId(partyMemberId, "Party Id");
        if (!partyMemberIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(partyMemberIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundPartyMember = await _unitOfWork.PartyMember.GetAsync(pm => pm.Id == partyMemberId);
            if (foundPartyMember == null)
                return ServiceResult<int>.Failure("Party Member not found");

            await _unitOfWork.PartyMember.RemoveAsync(foundPartyMember);

            return ServiceResult<int>.Success(foundPartyMember.Id);
        });
    }
}
