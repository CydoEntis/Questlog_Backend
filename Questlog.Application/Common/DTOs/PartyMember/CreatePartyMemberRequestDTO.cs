using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.PartyMember
{
    public class CreatePartyMemberRequestDTO
    {
        [Required]
        public int CharacterId { get; set; }
        [Required]
        public int PartyId { get; set; }
        [Required]
        public IdentityRole Role { get; set; }
    }
}
