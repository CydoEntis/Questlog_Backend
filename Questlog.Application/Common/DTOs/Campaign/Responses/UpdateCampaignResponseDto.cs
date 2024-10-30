namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record UpdateCampaignResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public DateTime DueDate { get; set; }
    }
}