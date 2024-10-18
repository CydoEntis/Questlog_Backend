using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;

using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Queries;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Linq.Expressions;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Request;
using Questlog.Application.Common.DTOs.GuildMember.Response;
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

    public async Task<ServiceResult<GetMemberResponseDto>> GetMember(int campaignId, int guildMemberId)
    {
        var idValidation = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
        if (!idValidation.IsSuccess) return ServiceResult<GetMemberResponseDto>.Failure(idValidation.ErrorMessage);

        var memberIdValidation = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidation.IsSuccess)
            return ServiceResult<GetMemberResponseDto>.Failure(memberIdValidation.ErrorMessage);

        return await HandleExceptions<GetMemberResponseDto>(async () =>
        {
            Member foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.CampaignId == campaignId && gm.Id == guildMemberId);

            if (foundMember == null)
            {
                return ServiceResult<GetMemberResponseDto>.Failure(" member not found.");
            }

            var guildMemberResponseDTO = _mapper.Map<GetMemberResponseDto>(foundMember);

            return ServiceResult<GetMemberResponseDto>.Success(guildMemberResponseDTO);
        });
    }


public async Task<ServiceResult<List<GetMemberResponseDto>>> GetAllMembers(int campaignId, MembersQueryParamsDto queryParams)
{
    // Validate the guild ID
    var idValidation = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
    if (!idValidation.IsSuccess)
        return ServiceResult<List<GetMemberResponseDto>>.Failure(idValidation.ErrorMessage);

    return await HandleExceptions<List<GetMemberResponseDto>>(async () =>
    {
        var options = new QueryOptions<Member>
        {
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            IsAscending = queryParams.OrderBy.Equals(OrderByOptions.Asc.ToString(), StringComparison.OrdinalIgnoreCase),
            FromDate = queryParams.JoinDateFrom,
            ToDate = queryParams.JoinDateTo,
            IncludeProperties = "User",
            DatePropertyName = "JoinedOn"
        };

        // Start with the base filter for CampaignId
        options.Filter = gm => gm.CampaignId == campaignId;

        // Handle additional search filters using the generic BuildSearchFilter method from BaseService
        if (!string.IsNullOrEmpty(queryParams.SearchBy) && !string.IsNullOrEmpty(queryParams.SearchValue))
        {
            options.Filter = options.Filter.And(BuildSearchFilter(
                queryParams.SearchBy, 
                queryParams.SearchValue, 
                new Dictionary<string, Func<Member, string>>
                {
                    { "displayname", gm => gm.User.DisplayName },
                    { "email", gm => gm.User.Email }
                }
            ));
        }

        // Setup ordering using the generic BuildOrdering method from BaseService
        options.OrderBy = BuildOrdering(
            queryParams.SortBy, 
            new Dictionary<string, Expression<Func<Member, object>>>
            {
                { "joinon", gm => gm.JoinedOn },
                { "displayname", gm => gm.User.DisplayName },
                { "email", gm => gm.User.Email }
            }
        );

        var guildMembers = await _unitOfWork.Member.GetAllAsync(options);

        if (guildMembers == null || !guildMembers.Any())
        {
            return ServiceResult<List<GetMemberResponseDto>>.Success(new List<GetMemberResponseDto>());
        }

        var guildMemberResponseDtos = _mapper.Map<List<GetMemberResponseDto>>(guildMembers);
        return ServiceResult<List<GetMemberResponseDto>>.Success(guildMemberResponseDtos);
    });
}



    // private Func<IQueryable<Member>, IOrderedQueryable<Member>> SortAndOrder(SortByOptions sortBy,
    //     OrderByOptions orderBy)
    // {
    //     var sortExpression = GetSortByExpression(sortBy);
    //
    //     return orderBy == OrderByOptions.Asc
    //         ? (q => q.OrderBy(sortExpression))
    //         : (q => q.OrderByDescending(sortExpression));
    // }
    //
    // private Expression<Func<Member, object>> GetSortByExpression(SortByOptions sortBy)
    // {
    //     return sortBy switch
    //     {
    //         SortByOptions.Id => gm => gm.Id,
    //         SortByOptions.Email => gm => gm.User.Email,
    //         SortByOptions.DisplayName => gm => gm.User.DisplayName,
    //         SortByOptions.Role => gm => gm.Role,
    //         SortByOptions.JoinOn => gm => gm.JoinedOn,
    //         _ => gm => gm.Id // Default sorting by Id
    //     };
    // }

    public async Task<ServiceResult<CreateMemberResponseDto>> CreateMember(CreateMemberRequestDto requestDto)
    {
        if (requestDto == null) ServiceResult<CreateMemberResponseDto>.Failure("Must provide a valid  Member");

        var userValidationResult = await ValidationHelper.ValidateUserIdAsync(requestDto.UserId, _userManager);
        if (!userValidationResult.IsSuccess)
            return ServiceResult<CreateMemberResponseDto>.Failure(userValidationResult.ErrorMessage);

        return await HandleExceptions<CreateMemberResponseDto>(async () =>
        {
            var guildMember = _mapper.Map<Member>(requestDto);
            guildMember.Role = RoleConstants.Member;

            var createdMember = await _unitOfWork.Member.CreateAsync(guildMember);

            var responseDTO = _mapper.Map<CreateMemberResponseDto>(createdMember);

            return ServiceResult<CreateMemberResponseDto>.Success(responseDTO);
        });
    }


    public async Task<ServiceResult<UpdateMemberRoleResponseDto>> UpdateMember(UpdateMemberRoleRequestDto roleRequestDto)
    {
        var guildValidationResult = ValidationHelper.ValidateObject(roleRequestDto, "Create  Request DTO");
        if (!guildValidationResult.IsSuccess)
            return ServiceResult<UpdateMemberRoleResponseDto>.Failure(guildValidationResult.ErrorMessage);

        var campaignIdValidationResult = ValidationHelper.ValidateId(roleRequestDto.Id, " Id");
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<UpdateMemberRoleResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);

        return await HandleExceptions<UpdateMemberRoleResponseDto>(async () =>
        {
            Member foundMember = await _unitOfWork.Member.GetAsync(gm => gm.Id == roleRequestDto.Id);

            if (foundMember == null)
                return ServiceResult<UpdateMemberRoleResponseDto>.Failure(" Member not found");

            var guildMember = _mapper.Map<Member>(roleRequestDto);

            foundMember.Role = guildMember.Role;
            foundMember.UpdatedOn = DateTime.UtcNow;

            await _unitOfWork.Member.UpdateAsync(foundMember);

            var guildMemberResponseDTO = _mapper.Map<UpdateMemberRoleResponseDto>(foundMember);

            return ServiceResult<UpdateMemberRoleResponseDto>.Success(guildMemberResponseDTO);
        });
    }

    public async Task<ServiceResult<int>> RemoveMember(int campaignId, int guildMemberId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, nameof(campaignId));
        if (!campaignIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(campaignIdValidationResult.ErrorMessage);

        var memberIdValidationResult = ValidationHelper.ValidateId(guildMemberId, nameof(guildMemberId));
        if (!memberIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(memberIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundMember =
                await _unitOfWork.Member.GetAsync(gm => gm.Id == guildMemberId && gm.CampaignId == campaignId);
            if (foundMember == null)
                return ServiceResult<int>.Failure(" Member not found");

            await _unitOfWork.Member.RemoveAsync(foundMember);

            return ServiceResult<int>.Success(foundMember.Id);
        });
    }
}