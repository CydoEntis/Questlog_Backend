﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Quest
{
    public record UpdateQuestOrderRequestDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int QuestBoardId { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public bool Completed { get; set; }
    }
}