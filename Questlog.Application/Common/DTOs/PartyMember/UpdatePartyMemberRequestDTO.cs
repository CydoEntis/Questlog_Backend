using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.PartyMember
{
    public record UpdatePartyMemberRequestDTO
    {
        [Required]
        public int Id { get; set; }
        public int CharacterId { get; set; }
        [Required]
        public int PartyId { get; set; }
        [Required]
        public int GuildId { get; set; }
        public string? Role { get; set; }
    }
}
