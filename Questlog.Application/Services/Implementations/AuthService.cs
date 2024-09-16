using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.DTOs;
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

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ITokenService tokenService)
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

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _unitOfWork.User.GetByEmail(loginRequestDTO.Email);
            bool isUserValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user is null || !isUserValid)
            {
                return new TokenDTO()
                {
                    AccessToken = ""
                };
            }

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = _tokenService.CreateAccessToken(user, jwtTokenId);
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id, jwtTokenId);

            TokenDTO tokenDTO = new TokenDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokenDTO;
        }

        public async Task<TokenDTO> Register(RegisterRequestDTO registerRequestDTO)
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
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

                if (result.Succeeded)
                {
                    var loginRequestDTO = new LoginRequestDTO
                    {
                        Email = registerRequestDTO.Email,
                        Password = registerRequestDTO.Password,

                    };

                    //var userToReturn =  await _unitOfWork.User.GetByEmail(user.Email);
                    return await Login(loginRequestDTO);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(errors);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(null, ex);
            }

        }
    }
}
