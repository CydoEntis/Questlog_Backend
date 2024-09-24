using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class QuestBoardDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string BoardColor { get; set; }
        public string MainQuestId { get; set; }

        public List<QuestDTO> Quests { get; set; } = new List<QuestDTO>();
    }
}
