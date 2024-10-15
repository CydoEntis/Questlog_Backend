using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Application.Common.DTOs.Party;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record GetGuildResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GuildLeaderId { get; set; }
        public string GuildLeader { get; set; }
        public string Color { get; set; }
        public int NumberOfMembers { get; set; }
        public int NumberOfParties { get; set; }
        public DateTime CreatedAt { get; set; }
        // public int Quests { get; set; }
    }
}
