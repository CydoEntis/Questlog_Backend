using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.Errors;
using Questlog.Application.Common.Exceptions;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Services.Interfaces;
using Questlog.Application.Services.IServices;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations;

public class AuthService : BaseService, IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger,
        UserManager<ApplicationUser> userManager, ITokenService tokenService) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<bool> CheckIfUsernameIsUnique(string username)
    {
        return await _unitOfWork.User.isUserUnique(username);
    }


    public async Task<ServiceResult<LoginDto>> Login(LoginCredentialsDto loginCredentialsDto)
    {
        var user = await _unitOfWork.User.GetAsync(u => u.Email == loginCredentialsDto.Email,
            includeProperties: "Avatar");
        bool isUserValid = await _userManager.CheckPasswordAsync(user, loginCredentialsDto.Password);

        if (!isUserValid)
        {
            return ServiceResult<LoginDto>.Failure("Invalid email or password.");
        }

        var jwtTokenId = $"JTI{Guid.NewGuid()}";
        var accessToken = _tokenService.CreateAccessToken(user, jwtTokenId);
        var refreshToken = await _tokenService.CreateRefreshToken(user.Id, jwtTokenId);

        var loginDto = _mapper.Map<LoginDto>(user);
        loginDto.Tokens = new TokenDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return ServiceResult<LoginDto>.Success(loginDto);
    }

    // public async Task<LoginDto> Login(LoginCredentialsDto loginCredentialsDto)
    // {
    //     var user = await _unitOfWork.User.GetByEmail(loginCredentialsDto.Email);
    //     bool isUserValid = await _userManager.CheckPasswordAsync(user, loginCredentialsDto.Password);
    //
    //     if (user is null || !isUserValid)
    //     {
    //         return new LoginDto()
    //         {
    //             UserId = "",
    //             Email = "",
    //             Tokens = new TokenDTO()
    //             {
    //                 AccessToken = "",
    //                 RefreshToken = "",
    //             },
    //         };
    //     }
    //
    //     var jwtTokenId = $"JTI{Guid.NewGuid()}";
    //     var accessToken = _tokenService.CreateAccessToken(user, jwtTokenId);
    //     var refreshToken = await _tokenService.CreateRefreshToken(user.Id, jwtTokenId);
    //
    //
    //     LoginDto loginDto = new LoginDto()
    //     {
    //         UserId = user.Id,
    //         Email = user.Email,
    //         DisplayName = user.DisplayName,
    //         Avatar = user.Avatar,
    //         CurrentExp = user.CurrentExp,
    //         CurrentLevel = user.CurrentLevel,
    //         ExpToNextLevel = user.ExpToNextLevel,
    //         Tokens = new TokenDTO()
    //         {
    //             AccessToken = accessToken,
    //             RefreshToken = refreshToken,
    //         },
    //     };
    //
    //     return loginDto;
    //
    // }

    public async Task<ServiceResult<LoginDto>> Register(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return ServiceResult<LoginDto>.Failure("A user with this email already exists.");
        }

        ApplicationUser user = new()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            NormalizedEmail = registerDto.Email.ToUpper(),
            NormalizedUserName = registerDto.Email.ToUpper(),
            DisplayName = registerDto.DisplayName,
            AvatarId = registerDto.AvatarId,
            CurrentLevel = 1,
            CurrentExp = 0,
            ExpToNextLevel = 100,
            CreatedAt = DateTime.Now,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var tierZeroAvatarIds = new List<int> { 1, 2, 3, 4 };
                foreach (var unlockedAvatar in tierZeroAvatarIds.Select(avatarId => new UnlockedAvatar
                         {
                             UserId = user.Id,
                             AvatarId = avatarId,
                             UnlockedAt = DateTime.Now,
                             IsUnlocked = true,
                         }))
                {
                    await _unitOfWork.UnlockedAvatar.CreateAsync(unlockedAvatar);
                }

                var loginRequestDto = new LoginCredentialsDto
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                };

                return await Login(loginRequestDto);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<LoginDto>.Failure("An unexpected error occurred during registration.");
        }

        return ServiceResult<LoginDto>.Failure("Registration failed for an unknown reason.");
    }
}