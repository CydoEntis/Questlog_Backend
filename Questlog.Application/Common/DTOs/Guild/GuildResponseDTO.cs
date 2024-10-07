using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.PartyMember;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild
{
    public record GuildResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreateGuildMemberResponseDTO> GuildMembers { get; set; }
    }
}
