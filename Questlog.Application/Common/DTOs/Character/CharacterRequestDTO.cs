using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Character
{
    public record CharacterRequestDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public Archetype  Archetype { get; set; }
        [Required]
        public int CurrentExp { get; set; }
        [Required]
        public int ExpToNextLevel { get; set; }
        [Required]
        public List<Unlockable>? Inventory { get; set; }
    }
}
