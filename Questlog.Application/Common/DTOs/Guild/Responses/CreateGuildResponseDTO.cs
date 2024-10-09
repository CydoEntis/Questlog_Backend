using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    // Currently Overkill, but adding for future possibilities.
    public record CreateGuildResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
