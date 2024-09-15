using Questlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Infrastructure.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool isUserUnique(string username);
        Task Login(LoginRequestDTO loginRequestDTO);
        Task Register(RegisterRequestDTO loginRequestDTO);
        Task RefreshAccessToken(TokenDTO tokenDTO);
        Task RevokeRefreshToken(TokenDTO tokenDTO);

    }
}
