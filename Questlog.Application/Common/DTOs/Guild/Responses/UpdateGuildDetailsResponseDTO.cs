using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    // Currently Overkill, but adding for future possibilities.
    public record UpdateGuildDetailsResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GuildLeaderId { get; set; }
        public string Color { get; set; }
    }
}
