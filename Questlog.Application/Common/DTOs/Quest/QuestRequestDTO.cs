using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Quest
{
    public class QuestRequestDTO
    {
        public string Title { get; set; }
        public string? Desc { get; set; }
        public string[]? Items { get; set; }
        public bool Completed { get; set; }
        public string Priority { get; set; }
        public int Order { get; set; }
    }
}

