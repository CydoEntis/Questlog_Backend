using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild
{
    public record UpdateGuildRequestDTO
    {
        [Required]
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string? Name { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string? Description { get; set; }

        public string? GuildLeaderId { get; set; }
    }
}
