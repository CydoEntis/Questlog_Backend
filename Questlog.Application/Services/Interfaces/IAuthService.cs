using Questlog.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.IServices
{
    public interface IAuthService
    {
        Task<bool> CheckIfUsernameIsUnique(string username);
        Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);

    }
}
