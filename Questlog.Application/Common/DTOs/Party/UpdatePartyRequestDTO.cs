using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Party
{
    public record UpdatePartyRequestDTO
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
