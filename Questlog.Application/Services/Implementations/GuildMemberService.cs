using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Request;
using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Queries;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Linq.Expressions;
using Questlog.Application.Common.Extensions;

namespace Questlog.Application.Services.Implementations;

public class GuildMemberService : BaseService, IGuildMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GuildMemberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ILogger<GuildMemberService> logger, IMapper mapper) : base(logger)
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
        if (!memberIdValidation.IsSuccess)
            return ServiceResult<GetGuildMemberResponseDTO>.Failure(memberIdValidation.ErrorMessage);

        return await HandleExceptions<GetGuildMemberResponseDTO>(async () =>
        {
            GuildMember foundGuildMember =
                await _unitOfWork.GuildMember.GetAsync(gm => gm.GuildId == guildId && gm.Id == guildMemberId);

            if (foundGuildMember == null)
            {
                return ServiceResult<GetGuildMemberResponseDTO>.Failure("Guild member not found.");
            }

            var guildMemberResponseDTO = _mapper.Map<GetGuildMemberResponseDTO>(foundGuildMember);

            return ServiceResult<GetGuildMemberResponseDTO>.Success(guildMemberResponseDTO);
        });
    }


public async Task<ServiceResult<List<GetGuildMemberResponseDTO>>> GetAllGuildMembers(int guildId, GuildMembersQueryParamsDTO queryParams)
{
    // Validate the guild ID
    var idValidation = ValidationHelper.ValidateId(guildId, nameof(guildId));
    if (!idValidation.IsSuccess)
        return ServiceResult<List<GetGuildMemberResponseDTO>>.Failure(idValidation.ErrorMessage);

    return await HandleExceptions<List<GetGuildMemberResponseDTO>>(async () =>
    {
        var options = new QueryOptions<GuildMember>
        {
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            IsAscending = queryParams.OrderBy.Equals(OrderByOptions.Asc.ToString(), StringComparison.OrdinalIgnoreCase),
            FromDate = queryParams.JoinDateFrom,
            ToDate = queryParams.JoinDateTo,
            IncludeProperties = "User",
            DatePropertyName = "JoinedOn"
        };

        // Start with the base filter for GuildId
        options.Filter = gm => gm.GuildId == guildId;

        // Handle additional search filters using the generic BuildSearchFilter method from BaseService
        if (!string.IsNullOrEmpty(queryParams.SearchBy) && !string.IsNullOrEmpty(queryParams.SearchValue))
        {
            options.Filter = options.Filter.And(BuildSearchFilter(
                queryParams.SearchBy, 
                queryParams.SearchValue, 
                new Dictionary<string, Func<GuildMember, string>>
                {
                    { "displayname", gm => gm.User.DisplayName },
                    { "email", gm => gm.User.Email }
                }
            ));
        }

        // Setup ordering using the generic BuildOrdering method from BaseService
        options.OrderBy = BuildOrdering(
            queryParams.SortBy, 
            new Dictionary<string, Expression<Func<GuildMember, object>>>
            {
                { "joinon", gm => gm.JoinedOn },
                { "displayname", gm => gm.User.DisplayName },
                { "email", gm => gm.User.Email }
            }
        );

        var guildMembers = await _unitOfWork.GuildMember.GetAllAsync(options);

        if (guildMembers == null || !guildMembers.Any())
        {
            return ServiceResult<List<GetGuildMemberResponseDTO>>.Success(new List<GetGuildMemberResponseDTO>());
        }

        var guildMemberResponseDtos = _mapper.Map<List<GetGuildMemberResponseDTO>>(guildMembers);
        return ServiceResult<List<GetGuildMemberResponseDTO>>.Success(guildMemberResponseDtos);
    });
}



    // private Func<IQueryable<GuildMember>, IOrderedQueryable<GuildMember>> SortAndOrder(SortByOptions sortBy,
    //     OrderByOptions orderBy)
    // {
    //     var sortExpression = GetSortByExpression(sortBy);
    //
    //     return orderBy == OrderByOptions.Asc
    //         ? (q => q.OrderBy(sortExpression))
    //         : (q => q.OrderByDescending(sortExpression));
    // }
    //
    // private Expression<Func<GuildMember, object>> GetSortByExpression(SortByOptions sortBy)
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

    public async Task<ServiceResult<GuildMemberResponseDTO>> CreateGuildMember(CreateGuildMemberRequestDTO requestDTO)
    {
        if (requestDTO == null) ServiceResult<GuildMemberResponseDTO>.Failure("Must provide a valid Guild Member");

        var userValidationResult = await ValidationHelper.ValidateUserIdAsync(requestDTO.UserId, _userManager);
        if (!userValidationResult.IsSuccess)
            return ServiceResult<GuildMemberResponseDTO>.Failure(userValidationResult.ErrorMessage);

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
        if (!guildValidationResult.IsSuccess)
            return ServiceResult<GuildMemberResponseDTO>.Failure(guildValidationResult.ErrorMessage);

        var guildIdValidationResult = ValidationHelper.ValidateId(requestDTO.Id, "Guild Id");
        if (!guildIdValidationResult.IsSuccess)
            return ServiceResult<GuildMemberResponseDTO>.Failure(guildIdValidationResult.ErrorMessage);

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
        if (!memberIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(memberIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundGuildMember =
                await _unitOfWork.GuildMember.GetAsync(gm => gm.Id == guildMemberId && gm.GuildId == guildId);
            if (foundGuildMember == null)
                return ServiceResult<int>.Failure("Guild Member not found");

            await _unitOfWork.GuildMember.RemoveAsync(foundGuildMember);

            return ServiceResult<int>.Success(foundGuildMember.Id);
        });
    }

    private Expression<Func<GuildMember, bool>> BuildSearchFilter(string searchBy, string searchValue)
    {
        return searchBy.ToLower() switch
        {
            "displayname" => gm => gm.User.DisplayName.Contains(searchValue),
            "email" => gm => gm.User.Email.Contains(searchValue),
            _ => gm => gm.Id.ToString().Contains(searchValue)
        };
    }

    private Func<IQueryable<GuildMember>, IOrderedQueryable<GuildMember>> BuildOrdering(string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "joinon" => query => query.OrderBy(gm => gm.JoinedOn),
            "displayname" => query => query.OrderBy(gm => gm.User.DisplayName),
            "email" => query => query.OrderBy(gm => gm.User.Email),
            _ => query => query.OrderBy(gm => gm.Id)
        };
    }
}