using Questlog.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class UserLevelService
    {
        private readonly IUserLevelRepository _userLevelRepository;

        public UserLevelService(IUserLevelRepository userLevelRepository)
        {
            _userLevelRepository = userLevelRepository;
        }


    }
}
