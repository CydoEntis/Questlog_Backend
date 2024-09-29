using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Auth
{
    public record LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
