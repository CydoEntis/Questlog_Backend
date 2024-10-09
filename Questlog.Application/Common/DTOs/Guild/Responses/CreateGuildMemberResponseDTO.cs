using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record CreateGuildMemberResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int GuildId { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime JoinedOn { get; set; }
    }
}
