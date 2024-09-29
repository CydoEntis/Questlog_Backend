using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Auth
{
    public record UserDTO
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
