﻿using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.DTOs.Character;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record GuildMemberResponseDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GuildId { get; set; }
        public string Role { get; set; }
        public DateTime JoinedOn { get; set; }

        public CharacterResponseDTO Character { get; set; }
    }
}