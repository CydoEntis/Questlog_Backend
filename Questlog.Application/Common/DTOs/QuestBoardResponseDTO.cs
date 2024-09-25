using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class QuestBoardResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string BoardColor { get; set; }

        public List<QuestRequestDTO> Quests { get; set; } = new List<QuestRequestDTO>();
    }
}
