using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questlog.Application.Common.DTOs.Quest;

namespace Questlog.Application.Common.DTOs.QuestBoard
{
    public class QuestBoardResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string BoardColor { get; set; }

        public List<QuestResponseDTO> Quests { get; set; } = new List<QuestResponseDTO>();
    }
}
