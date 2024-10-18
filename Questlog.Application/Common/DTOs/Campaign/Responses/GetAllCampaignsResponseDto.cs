namespace Questlog.Application.Common.DTOs.Campaign.Responses
{
    public record GetAllCampaignsResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string Owner { get; set; }
        public string Color { get; set; }
        public int NumberOfMembers { get; set; }
        public DateTime CreatedAt { get; set; }
        // public int Quests { get; set; }
    }
}
