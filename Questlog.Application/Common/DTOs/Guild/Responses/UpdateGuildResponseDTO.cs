using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    // Currently Overkill, but adding for future possibilities.
    public record UpdateGuildResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
