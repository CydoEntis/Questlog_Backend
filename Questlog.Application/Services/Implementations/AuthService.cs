
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.DTOs.Character;
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
        private readonly ICharacterService _characterService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ErrorMapper _errorMapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ITokenService tokenService, ICharacterService characterService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _errorMapper = new();
            _characterService = characterService;
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
                    Tokens = new TokenDTO()
                    {
                        AccessToken = "",
                        RefreshToken = "",
                    },
                    Character = new CharacterResponseDTO(),
                };
            }

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = _tokenService.CreateAccessToken(user, jwtTokenId);
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id, jwtTokenId);

            var characterResponseDTO = _mapper.Map<CharacterResponseDTO>(user.Character);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Email = user.Email,
                Tokens = new TokenDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                },
                Character = characterResponseDTO,
            };

            return loginResponseDTO;

        }

        public async Task<LoginResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDTO.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("A user with this email already exists.", nameof(registerRequestDTO.Email));
            }

            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                NormalizedUserName = registerRequestDTO.Email.ToUpper(),
                CreatedAt = DateTime.Now,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

                if (result.Succeeded)
                {
                    var newCharacter = new Character
                    {
                        DisplayName = registerRequestDTO.DisplayName,
                        Archetype = registerRequestDTO.Archetype,
                        User = user,
                    };

                    await _characterService.CreateCharacterAsync(user.Id, newCharacter);

                    user.CharacterId = newCharacter.Id;

                    await _userManager.UpdateAsync(user);

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
                throw;
            }
        }


    }
}
