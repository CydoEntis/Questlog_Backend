using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class MainQuestResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string QuestColor { get; set; }
        public int Order { get; set; }

        public List<CreateQuestBoardRequestDTO> QuestBoards { get; set; } = new List<CreateQuestBoardRequestDTO>();
    }
}
