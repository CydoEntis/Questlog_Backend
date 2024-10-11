using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Requests
{
    public record UpdateGuildDetailsRequestDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string? Name { get; set; }
        [MinLength(5)]
        [MaxLength(100)]
        [Required]
        public string? Description { get; set; }
        [Required]
        public string GuildLeaderId { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
