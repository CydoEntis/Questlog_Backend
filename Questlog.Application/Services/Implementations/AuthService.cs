using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.IServices;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> CheckIfUsernameIsUnique(string username)
        {
            return await _unitOfWork.User.isUserUnique(username);
        }

        public Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
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
                    var userToReturn =  await _unitOfWork.User.GetByUserName(user.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new UserDTO();
        }
    }
}
