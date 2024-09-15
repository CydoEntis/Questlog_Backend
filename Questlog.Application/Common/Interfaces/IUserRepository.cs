using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        bool isUserUnique(string email);
        //Task RefreshAccessToken(TokenDTO tokenDTO);
        //Task RevokeRefreshToken(TokenDTO tokenDTO);
        Task<ApplicationUser> GetByUserName(string userName);

    }
}
