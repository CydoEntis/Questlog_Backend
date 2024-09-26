using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public TokenDTO TokenDTO { get; set; }
    }
}
