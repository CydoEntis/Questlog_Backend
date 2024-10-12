using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Character
{
    public record CharacterResponseDTO
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public Avatar  Avatar { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentExp { get; set; }
        public int ExpToNextLevel { get; set; }
        public List<Unlockable>? Inventory { get; set; }
    }
}
