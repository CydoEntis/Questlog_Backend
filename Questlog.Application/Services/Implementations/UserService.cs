using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.User;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUnitOfWork unitOfWork,
        ILogger<UserService> logger, IMapper mapper, UserManager<ApplicationUser> userManager) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ServiceResult<UserDto>> GetUserById(string userId)
    {
        try
        {
            var foundUser =
                await _unitOfWork.User.GetAsync(u => u.Id == userId, includeProperties: "Avatar,UnlockedAvatars,UnlockedAvatars.Avatar");
            if (foundUser == null)
            {
                return ServiceResult<UserDto>.Failure("User not found.");
            }

            var userDto = _mapper.Map<UserDto>(foundUser);

            return ServiceResult<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<UserDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }
}