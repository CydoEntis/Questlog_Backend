using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.ApplicationUser.Response
{
    public record GetApplicationUserResponseDTO
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int Avatar { get; set; }
        public int CurrentLevel { get; set; }
    }
}
