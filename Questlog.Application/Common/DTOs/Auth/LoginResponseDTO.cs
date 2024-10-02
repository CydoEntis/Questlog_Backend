using Questlog.Application.Common.DTOs.UserLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Auth
{
    public record LoginResponseDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public TokenDTO Tokens { get; set; }
        public UserLevelResponseDTO? UserLevel { get; set; }
    }
}
