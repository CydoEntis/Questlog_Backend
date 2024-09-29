using Questlog.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.QuestBoard
{
    public record QuestBoardFilterParams
    {
        public int? Id { get; set; }
        public int? MainQuestId { get; set; }
        public SortOrder? Order { get; set; } = SortOrder.Asc;
    }
}
