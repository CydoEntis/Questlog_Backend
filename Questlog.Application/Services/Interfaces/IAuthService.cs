using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.IServices;

public interface IAuthService
{
    Task<bool> CheckIfUsernameIsUnique(string username);
    Task<ServiceResult<LoginDto>> Login(LoginCredentialsDto loginCredentialsDto);
    Task<ServiceResult<LoginDto>> Register(RegisterDto registerDto);
}