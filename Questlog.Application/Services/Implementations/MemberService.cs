using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Member;
using Questlog.Application.Common.Extensions;

namespace Questlog.Application.Services.Implementations;

public class MemberService : BaseService, IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public MemberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ILogger<MemberService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResult<MemberDto>> GetMember(int partyId, int guildMemberId)
    {
        var idValidation = ValidationHelper.ValidateId(partyId, nameof(partyId));
        if (!idValidation.IsSuccess) return ServiceResult<MemberDto>.Failure(idValidation.ErrorMessage);

        var memberIdValidation = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidation.IsSuccess)
            return ServiceResult<MemberDto>.Failure(memberIdValidation.ErrorMessage);

        return await HandleExceptions<MemberDto>(async () =>
        {
            Member foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.PartyId == partyId && gm.Id == guildMemberId);

            if (foundMember == null)
            {
                return ServiceResult<MemberDto>.Failure(" member not found.");
            }

            var guildMemberResponseDTO = _mapper.Map<MemberDto>(foundMember);

            return ServiceResult<MemberDto>.Success(guildMemberResponseDTO);
        });
    }


    public async Task<ServiceResult<List<MemberDto>>> GetAllMembers(int partyId)
    {
        return await HandleExceptions<List<MemberDto>>(async () =>
        {
            var members = await _unitOfWork.Member.GetAllAsync(m => m.PartyId == partyId);

            var memberResponseDtos = _mapper.Map<List<MemberDto>>(members);


            return ServiceResult<List<MemberDto>>.Success(memberResponseDtos);
        });
    }

    public async Task<ServiceResult<PaginatedResult<MemberDto>>> GetAllPaginatedMembers(int partyId,
        QueryParamsDto queryParams)
    {
        return await HandleExceptions<PaginatedResult<MemberDto>>(async () =>
        {
            var options = new QueryOptions<Member>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                OrderBy = queryParams.OrderBy,
                SortBy = queryParams.SortBy,
                FilterDate = queryParams.FilterDate,
                IncludeProperties = "User,User.Avatar",
                Filter = c => c.PartyId == partyId
            };

            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                options.Filter = options.Filter.And(c => c.User.DisplayName.Contains(queryParams.Search));
            }

            var paginatedResult = await _unitOfWork.Member.GetPaginated(options);

            var memberResponseDtos = _mapper.Map<List<MemberDto>>(paginatedResult.Items);

            // Create a new PaginatedResult for the DTOs
            var result = new PaginatedResult<MemberDto>(memberResponseDtos, paginatedResult.TotalItems,
                paginatedResult.CurrentPage, queryParams.PageSize);

            return ServiceResult<PaginatedResult<MemberDto>>.Success(result);
        });
    }

    public async Task<ServiceResult<MemberDto>> CreateMember(CreateMemberDto requestDto)
    {
        if (requestDto == null) ServiceResult<MemberDto>.Failure("Must provide a valid  Member");

        var userValidationResult = await ValidationHelper.ValidateUserIdAsync(requestDto.UserId, _userManager);
        if (!userValidationResult.IsSuccess)
            return ServiceResult<MemberDto>.Failure(userValidationResult.ErrorMessage);

        return await HandleExceptions<MemberDto>(async () =>
        {
            var guildMember = _mapper.Map<Member>(requestDto);
            guildMember.Role = RoleConstants.Member;

            var createdMember = await _unitOfWork.Member.CreateAsync(guildMember);

            var responseDTO = _mapper.Map<MemberDto>(createdMember);

            return ServiceResult<MemberDto>.Success(responseDTO);
        });
    }


    public async Task<ServiceResult<MemberDto>> UpdateMember(
        UpdateMemberDto requestDto)
    {
        var guildValidationResult = ValidationHelper.ValidateObject(requestDto, "Create  Request DTO");
        if (!guildValidationResult.IsSuccess)
            return ServiceResult<MemberDto>.Failure(guildValidationResult.ErrorMessage);

        var partyIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, " Id");
        if (!partyIdValidationResult.IsSuccess)
            return ServiceResult<MemberDto>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<MemberDto>(async () =>
        {
            Member foundMember = await _unitOfWork.Member.GetAsync(gm => gm.Id == requestDto.Id);

            if (foundMember == null)
                return ServiceResult<MemberDto>.Failure(" Member not found");

            var guildMember = _mapper.Map<Member>(requestDto);

            foundMember.Role = guildMember.Role;
            foundMember.UpdatedOn = DateTime.UtcNow;

            await _unitOfWork.Member.UpdateAsync(foundMember);

            var guildMemberResponseDTO = _mapper.Map<MemberDto>(foundMember);

            return ServiceResult<MemberDto>.Success(guildMemberResponseDTO);
        });
    }

    public async Task<ServiceResult<int>> RemoveMember(int partyId, int guildMemberId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, nameof(partyId));
        if (!partyIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(partyIdValidationResult.ErrorMessage);

        var memberIdValidationResult = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(memberIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.Id == guildMemberId && gm.PartyId == partyId);
            if (foundMember == null)
                return ServiceResult<int>.Failure(" Member not found");

            await _unitOfWork.Member.RemoveAsync(foundMember);

            return ServiceResult<int>.Success(foundMember.Id);
        });
    }


    public async Task<ServiceResult<string>> GenerateInviteLink(int partyId)
    {
        var partyValidationResult = ValidationHelper.ValidateId(partyId, nameof(partyId));
        if (!partyValidationResult.IsSuccess)
            return ServiceResult<string>.Failure(partyValidationResult.ErrorMessage);

        var token = Guid.NewGuid().ToString();

        var expirationTime = DateTime.UtcNow.AddMinutes(15);

        await _unitOfWork.InviteToken.CreateAsync(new InviteToken
        {
            Token = token,
            CampaignId = partyId,
            CreatedOn = DateTime.UtcNow,
            Expiration = expirationTime,
        });

        var inviteToken = token;

        return ServiceResult<string>.Success(token);
    }

    public async Task<ServiceResult<string>> AcceptInvite(string token, string userId)
    {
        try
        {
            var inviteToken = await _unitOfWork.InviteToken.GetAsync(t => t.Token == token);

            if (inviteToken.Expiration < DateTime.UtcNow)
            {
                inviteToken.IsExpired = true;
                await _unitOfWork.InviteToken.SaveAsync();
                return ServiceResult<string>.Failure("Invite token has expired.");
            }

            var newMember = new Member()
            {
                PartyId = inviteToken.CampaignId,
                Role = RoleConstants.Member,
                JoinedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                UserId = userId
            };

            await _unitOfWork.Member.CreateAsync(newMember);

            await _unitOfWork.InviteToken.RemoveAsync(inviteToken);
            await _unitOfWork.SaveAsync();

            return ServiceResult<string>.Success("You have successfully joined the party.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while accepting the invite for token {Token}.", token);
            return ServiceResult<string>.Failure("An error occurred while processing your request.");
        }
    }


    public async Task<ServiceResult<List<MemberDto>>> UpdateMemberRoles(int partyId,
        List<MemberRole> updatedMemberRoles, string currentUserId)
    {
        var partyValidation = ValidationHelper.ValidateId(partyId, nameof(partyId));
        if (!partyValidation.IsSuccess)
            return ServiceResult<List<MemberDto>>.Failure("Invalid party ID.");

        if (updatedMemberRoles == null || !updatedMemberRoles.Any())
            return ServiceResult<List<MemberDto>>.Failure("No member role updates provided.");

        try
        {
            var currentUser = await _unitOfWork.Member.GetAsync(m => m.PartyId == partyId && m.UserId == currentUserId);

            if (currentUser == null || string.IsNullOrEmpty(currentUser.Role))
                return ServiceResult<List<MemberDto>>.Failure("User not found");

            if (currentUser.Role != RoleConstants.Creator && currentUser.Role != RoleConstants.Maintainer)
                return ServiceResult<List<MemberDto>>.Failure("User lacks role to update members");

            var memberIds = updatedMemberRoles.Select(m => m.Id).ToList();
            
            var membersToUpdate = await _unitOfWork.Member.GetAllAsync(m => m.PartyId == partyId && memberIds.Contains(m.Id), includeProperties: "User,User.Avatar");
            if (membersToUpdate == null || !membersToUpdate.Any())
                return ServiceResult<List<MemberDto>>.Failure("No matching members found for the provided IDs.");

            var updatedMembers = new List<MemberDto>();

            foreach (var updatedMemberRole in updatedMemberRoles)
            {
                var member = membersToUpdate.FirstOrDefault(m => m.Id == updatedMemberRole.Id);
                if (member != null)
                {
                    member.Role = updatedMemberRole.Role;
                    member.UpdatedOn = DateTime.UtcNow;

                    await _unitOfWork.Member.UpdateAsync(member);
                    updatedMembers.Add(_mapper.Map<MemberDto>(member));
                }
            }

            return ServiceResult<List<MemberDto>>.Success(updatedMembers);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the members' roles.");
            return ServiceResult<List<MemberDto>>.Failure("An error occurred while updating the roles.");
        }
    }


    public async Task<ServiceResult<string>> ChangeCreatorRole(int partyId, int newCreatorId, string currentUserId)
    {
        var partyValidation = ValidationHelper.ValidateId(partyId, nameof(partyId));
        var newCreatorValidation = ValidationHelper.ValidateId(newCreatorId, nameof(newCreatorId));
        if (!partyValidation.IsSuccess || !newCreatorValidation.IsSuccess)
            return ServiceResult<string>.Failure("Invalid party or member ID.");

        try
        {
            var currentCreator =
                await _unitOfWork.Member.GetAsync(m => m.PartyId == partyId && m.UserId == currentUserId);
            if (currentCreator == null || currentCreator.Role != RoleConstants.Creator)
                return ServiceResult<string>.Failure("Only the current 'Creator' can transfer the 'Creator' role.");

            var newCreator = await _unitOfWork.Member.GetAsync(m => m.PartyId == partyId && m.Id == newCreatorId);
            if (newCreator == null)
                return ServiceResult<string>.Failure("New creator not found.");

            var party = await _unitOfWork.Party.GetAsync(p => p.Id == partyId);
            if (party == null)
                return ServiceResult<string>.Failure("Party not found.");

            currentCreator.Role = RoleConstants.Member;
            newCreator.Role = RoleConstants.Creator;
            currentCreator.UpdatedOn = DateTime.UtcNow;
            newCreator.UpdatedOn = DateTime.UtcNow;

            party.CreatorId = newCreator.UserId;
            party.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Member.UpdateAsync(currentCreator);
            await _unitOfWork.Member.UpdateAsync(newCreator);
            await _unitOfWork.Party.UpdateAsync(party);

            return ServiceResult<string>.Success(
                $"Role transferred. {newCreator.User.DisplayName} is now the 'Creator'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while transferring the creator role.");
            return ServiceResult<string>.Failure("An error occurred while transferring the creator role.");
        }
    }
}