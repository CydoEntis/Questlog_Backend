using Questlog.Application.Common.DTOs.Auth;

namespace Questlog.Application.Services.IServices;

public interface IAuthService
{
    Task<bool> CheckIfUsernameIsUnique(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    // Fix registering
    Task<LoginResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
}
