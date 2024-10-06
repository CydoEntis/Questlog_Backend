using Microsoft.AspNetCore.Identity;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Party
{
    public record CreatedPartyMemberResponseDTO
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public int PartyId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public IdentityRole Role { get; set; }
    }
}
