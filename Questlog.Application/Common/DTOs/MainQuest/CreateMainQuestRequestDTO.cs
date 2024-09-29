using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.MainQuest
{
    public record CreateMainQuestRequestDTO
    {

        [Required]
        public string Title { get; set; }
        [Required]
        public string QuestColor { get; set; }
        [Required]
        public int Order { get; set; }

        public List<CreateQuestBoardRequestDTO> QuestBoards { get; set; } = new List<CreateQuestBoardRequestDTO>();
    }
}
