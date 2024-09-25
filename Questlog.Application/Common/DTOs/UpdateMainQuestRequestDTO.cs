using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs
{
    public class UpdateMainQuestRequestDTO
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? QuestColor { get; set; }
    }
}
