using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class MainQuestDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string QuestColor { get; set; }
        public int Order { get; set; }

        // Consider sending IDs or relevant information of quest boards
        public List<QuestBoardDTO> QuestBoards { get; set; } = new List<QuestBoardDTO>();
    }
}
