using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Extensions;
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


    public PartyService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ILogger<PartyService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PartyDto>> GetPartyById(int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
        if (!partyIdValidationResult.IsSuccess)
            return ServiceResult<PartyDto>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<PartyDto>(async () =>
        {
            var foundParty =
                await _unitOfWork.Party.GetAsync(g => g.Id == partyId,
                    includeProperties: "Members,Members.User,Members.User.Avatar");

            if (foundParty == null)
            {
                return ServiceResult<PartyDto>.Failure("Party not found.");
            }

            var partyDto = _mapper.Map<PartyDto>(foundParty);

            return ServiceResult<PartyDto>.Success(partyDto);
        });
    }

    public async Task<ServiceResult<PaginatedResult<PartyDto>>> GetAllParties(string userId,
        QueryParamsDto queryParams)
    {
        try
        {
            var options = new QueryOptions<Party>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                OrderBy = queryParams.OrderBy,
                OrderOn = queryParams.OrderOn,
                IncludeProperties = "Members,Members.User,Members.User.Avatar",
                Filter = c => c.Members.Any(m => m.UserId == userId)
            };

            if (!string.IsNullOrEmpty(queryParams.SearchValue))
            {
                options.Filter = options.Filter.And(c => c.Title.Contains(queryParams.SearchValue));
            }

            var paginatedResult = await _unitOfWork.Party.GetPaginatedPartiesAsync(options);

            var partyDto = _mapper.Map<List<PartyDto>>(paginatedResult.Items);

            var result = new PaginatedResult<PartyDto>(partyDto, paginatedResult.TotalItems,
                paginatedResult.CurrentPage, queryParams.PageSize);

            return ServiceResult<PaginatedResult<PartyDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ServiceResult<PaginatedResult<PartyDto>>.Failure(ex.InnerException.ToString());
        }
    }


    public async Task<ServiceResult<PartyDto>> CreateParty(string userId,
        CreatePartyDto requestDto)
    {
        try
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess)
                return ServiceResult<PartyDto>.Failure(userValidationResult.ErrorMessage);

            var partyValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Party Request DTO");
            if (!partyValidationResult.IsSuccess)
                return ServiceResult<PartyDto>.Failure(partyValidationResult.ErrorMessage);


            var party = _mapper.Map<Party>(requestDto);
            party.OwnerId = userId;

            Party createdParty = await _unitOfWork.Party.CreateAsync(party);

            var partyOwner = new Member
            {
                UserId = userId,
                PartyId = createdParty.Id,
                Role = RoleConstants.Owner,
                JoinedOn = DateTime.UtcNow,
            };

            await _unitOfWork.Member.CreateAsync(partyOwner);

            var partyWithLeader = await _unitOfWork.Party
                .GetAsync(g => g.Id == party.Id, includeProperties: "Members,Members.User");

            var partyDto = _mapper.Map<PartyDto>(partyWithLeader);

            return ServiceResult<PartyDto>.Success(partyDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<PartyDto>.Failure(
                ex.InnerException.ToString());
        }
    }


    public async Task<ServiceResult<PartyDto>> UpdateParty(
        UpdatePartyDto requestDto, string userId)
    {
        try
        {
            var partyValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Party Request DTO");
            if (!partyValidationResult.IsSuccess)
                return ServiceResult<PartyDto>.Failure(partyValidationResult.ErrorMessage);

            var partyIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Party Id");
            if (!partyIdValidationResult.IsSuccess)
                return ServiceResult<PartyDto>.Failure(partyIdValidationResult.ErrorMessage);

            if (!await IsUserPartyOwner(requestDto.Id, userId))
            {
                return ServiceResult<PartyDto>.Failure(
                    "User is not authorized to update the party leader.");
            }


            var foundParty = await _unitOfWork.Party.GetAsync(g => g.Id == requestDto.Id && g.OwnerId == userId);

            if (foundParty == null)
            {
                return ServiceResult<PartyDto>.Failure("Party not found.");
            }


            foundParty.Title = requestDto.Title.Trim();
            foundParty.Description = requestDto.Description.Trim();
            foundParty.Color = requestDto.Color;
            foundParty.UpdatedAt = DateTime.UtcNow;
            if (requestDto.DueDate.HasValue)
            {
                foundParty.DueDate = requestDto.DueDate.Value;
            }

            await _unitOfWork.Party.UpdateAsync(foundParty);

            var responseDto = _mapper.Map<PartyDto>(foundParty);

            return ServiceResult<PartyDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<PartyDto>.Failure(ex.InnerException.ToString());
        }
    }

    public async Task<ServiceResult<int>> DeleteParty(int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
        if (!partyIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundParty = await _unitOfWork.Party.GetAsync(g => g.Id == partyId);

            if (foundParty == null)
            {
                return ServiceResult<int>.Failure("Party not found.");
            }

            await _unitOfWork.Party.RemoveAsync(foundParty);

            return ServiceResult<int>.Success(foundParty.Id);
        });
    }

    private async Task<bool> IsUserPartyOwner(int partyId, string userId)
    {
        var party = await _unitOfWork.Party.GetAsync(g => g.Id == partyId);
        return party?.OwnerId == userId;
    }
}