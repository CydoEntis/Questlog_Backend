using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Requests
{
    public record UpdateGuildLeaderRequestDTO
    {
        public int Id { get; set; }
        public string GuildLeaderId { get; set; }
    }
}
