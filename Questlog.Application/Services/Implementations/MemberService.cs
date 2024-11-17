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

    public async Task<ServiceResult<MemberDto>> GetMember(int campaignId, int guildMemberId)
    {
        var idValidation = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
        if (!idValidation.IsSuccess) return ServiceResult<MemberDto>.Failure(idValidation.ErrorMessage);

        var memberIdValidation = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidation.IsSuccess)
            return ServiceResult<MemberDto>.Failure(memberIdValidation.ErrorMessage);

        return await HandleExceptions<MemberDto>(async () =>
        {
            Member foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.PartyId == campaignId && gm.Id == guildMemberId);

            if (foundMember == null)
            {
                return ServiceResult<MemberDto>.Failure(" member not found.");
            }

            var guildMemberResponseDTO = _mapper.Map<MemberDto>(foundMember);

            return ServiceResult<MemberDto>.Success(guildMemberResponseDTO);
        });
    }


    public async Task<ServiceResult<List<MemberDto>>> GetAllMembers(int campaignId)
    {
        return await HandleExceptions<List<MemberDto>>(async () =>
        {
            var members = await _unitOfWork.Member.GetAllAsync(m => m.PartyId == campaignId);

            var memberResponseDtos = _mapper.Map<List<MemberDto>>(members);


            return ServiceResult<List<MemberDto>>.Success(memberResponseDtos);
        });
    }

    public async Task<ServiceResult<PaginatedResult<MemberDto>>> GetAllPaginatedMembers(int campaignId,
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
                IncludeProperties = "User",
                Filter = c => c.PartyId == campaignId
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

        var campaignIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, " Id");
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<MemberDto>.Failure(campaignIdValidationResult.ErrorMessage);

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

    public async Task<ServiceResult<int>> RemoveMember(int campaignId, int guildMemberId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(campaignIdValidationResult.ErrorMessage);

        var memberIdValidationResult = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(memberIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.Id == guildMemberId && gm.PartyId == campaignId);
            if (foundMember == null)
                return ServiceResult<int>.Failure(" Member not found");

            await _unitOfWork.Member.RemoveAsync(foundMember);

            return ServiceResult<int>.Success(foundMember.Id);
        });
    }

    public async Task<ServiceResult<string>> GenerateInviteLink(int campaignId)
    {
        var campaignValidationResult = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
        if (!campaignValidationResult.IsSuccess)
            return ServiceResult<string>.Failure(campaignValidationResult.ErrorMessage);

        var token = Guid.NewGuid().ToString();

        var expirationTime = DateTime.UtcNow.AddMinutes(15);

        await _unitOfWork.InviteToken.CreateAsync(new InviteToken
        {
            Token = token,
            CampaignId = campaignId,
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

            return ServiceResult<string>.Success("You have successfully joined the campaign.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while accepting the invite for token {Token}.", token);
            return ServiceResult<string>.Failure("An error occurred while processing your request.");
        }
    }
}