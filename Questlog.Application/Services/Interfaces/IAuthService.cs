using Questlog.Application.Common.DTOs.Auth;

namespace Questlog.Application.Services.IServices;

public interface IAuthService
{
    Task<bool> CheckIfUsernameIsUnique(string username);
    Task<LoginDto> Login(LoginCredentialsDto loginCredentialsDto);
    // Fix registering
    Task<LoginDto> Register(RegisterDto registerDto);
}
