using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild
{
    public record CreateGuildResponseDTO
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public List<GuildMemberResponseDTO> GuildMembers { get; set; }

    }
}
