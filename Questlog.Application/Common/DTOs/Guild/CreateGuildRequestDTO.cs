using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questlog.Application.Common.DTOs.Party;

namespace Questlog.Application.Common.DTOs.Guild
{
    public record CreateGuildRequestDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Description { get; set; }

        public CreatePartyRequestDTO Party { get; set; }
    }
}
