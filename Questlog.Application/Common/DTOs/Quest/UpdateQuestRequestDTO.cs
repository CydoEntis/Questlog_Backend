using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questlog.Application.Common.DTOs.Quest;

namespace Questlog.Application.Common.DTOs.Quest
{
    public record UpdateQuestRequestDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Desc { get; set; }
        public string[]? Items { get; set; }
        public string Priority { get; set; }
        public int Order { get; set; }
        public int QuestBoardId { get; set; }

    }
}
