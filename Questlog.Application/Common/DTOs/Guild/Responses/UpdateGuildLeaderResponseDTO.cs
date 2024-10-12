using Questlog.Application.Common.DTOs.GuildMember.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record UpdateGuildLeaderResponseDTO
    {
        public GuildMemberResponseDTO PreviousGuildLeader { get; set; }
        public GuildMemberResponseDTO NewGuildLeader { get; set; }
        public int GuildId { get; set; }
    }
}
