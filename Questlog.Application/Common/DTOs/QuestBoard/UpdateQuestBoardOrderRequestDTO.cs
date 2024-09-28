using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.QuestBoard
{
    public class UpdateQuestBoardOrderRequestDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int MainQuestId { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
