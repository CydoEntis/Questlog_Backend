﻿using Questlog.Application.Common.DTOs.GuildMember.Response;

namespace Questlog.Application.Common.DTOs.Campaign.Responses
{
    public record GetCampaignResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string Owner { get; set; }
        public string Color { get; set; }
        public int NumberOfMembers { get; set; }
        public List<GetMemberAvatarResponseDto> Members { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DueDate { get; set; }


        // public int Quests { get; set; }
    }
}