using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.PartyMember;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild
{
    public class GuildResponseDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Description { get; set; }


        public virtual List<PartyResponseDTO> Parties { get; set; } = new List<PartyResponseDTO>();

    }
}
