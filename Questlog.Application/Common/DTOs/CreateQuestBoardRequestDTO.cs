using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class CreateQuestBoardRequestDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public string BoardColor { get; set; }

        public List<QuestRequestDTO> Quests { get; set; } = new List<QuestRequestDTO>();
    }
}
