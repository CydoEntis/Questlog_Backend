using Microsoft.AspNetCore.Identity;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.PartyMember
{
    public record PartyMemberResponseDTO
    {
        [Required]
        public int Id { get; set; }

        public int CharacterId { get; set; }

        [Required]
        public int PartyId { get; set; }

        [Required]
        public IdentityRole Role { get; set; }

        [Required]
        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
    }
}
