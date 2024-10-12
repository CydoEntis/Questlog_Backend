using Questlog.Application.Common.DTOs.ApplicationUser.Response;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.GuildMember.Response
{
    public record GetGuildMemberResponseDTO
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int Avatar { get; set; }
        public int CurrentLevel { get; set; }
        public DateTime JoinedOn { get; set; }
    }
}
