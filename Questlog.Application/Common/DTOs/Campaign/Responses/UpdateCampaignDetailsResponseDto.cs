﻿namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record UpdateCampaignDetailsResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}