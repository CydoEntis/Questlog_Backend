using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.DTOs.UserLevel;
using Questlog.Application.Common.Errors;
using Questlog.Application.Common.Exceptions;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
using Questlog.Application.Services.IServices;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ErrorMapper _errorMapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _errorMapper = new();
        }

        public async Task<bool> CheckIfUsernameIsUnique(string username)
        {
            return await _unitOfWork.User.isUserUnique(username);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _unitOfWork.User.GetByEmail(loginRequestDTO.Email);
            bool isUserValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user is null || !isUserValid)
            {
                return new LoginResponseDTO()
                {
                    Email = "",
                    DisplayName = "",
                    Tokens = new TokenDTO()
                    {
                        AccessToken = "",
                        RefreshToken = "",
                    },
                };
                //return new TokenDTO()
                //{
                //    AccessToken = ""
                //};
            }

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = _tokenService.CreateAccessToken(user, jwtTokenId);
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id, jwtTokenId);

            var userLevelDTO = _mapper.Map<UserLevelResponseDTO>(user.UserLevel);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Tokens = new TokenDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                },
                UserLevel = userLevelDTO,
            };

            return loginResponseDTO;
            //TokenDTO tokenDTO = new TokenDTO()
            //{
            //    AccessToken = accessToken,
            //    RefreshToken = refreshToken,
            //};

            //return tokenDTO;
        }

        public async Task<LoginResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                DisplayName = registerRequestDTO.DisplayName,
                NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                NormalizedUserName = registerRequestDTO.Email.ToUpper(),
                CreatedAt = DateTime.Now,
            };

            try
            {
                // Create the user
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

                if (result.Succeeded)
                {
                    // Create UserLevel for the newly registered user
                    var userLevel = new UserLevel
                    {
                        ApplicationUserId = user.Id, // Set the foreign key
                        CurrentLevel = 1,
                        CurrentExp = 0
                    };

                    // Add the UserLevel to the database
                    await _unitOfWork.UserLevel.CreateAsync(userLevel);
                    await _unitOfWork.SaveAsync(); // Save changes to the database

                    var loginRequestDTO = new LoginRequestDTO
                    {
                        Email = registerRequestDTO.Email,
                        Password = registerRequestDTO.Password,
                    };

                    return await Login(loginRequestDTO);
                }
                else
                {
                    _errorMapper.MapErrors(result);
                    throw new RegistrationException(_errorMapper.Errors);
                }
            }
            catch (Exception ex)
            {
                throw ex; // You may want to handle exceptions more gracefully
            }
        }

    }
}
