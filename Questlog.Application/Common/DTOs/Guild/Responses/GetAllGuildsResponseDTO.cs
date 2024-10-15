using Questlog.Application.Common.DTOs.GuildMember.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record GetAllGuildsResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int NumberOfMembers { get; set; }
        public int NumberOfParties { get; set; }
    }
}
