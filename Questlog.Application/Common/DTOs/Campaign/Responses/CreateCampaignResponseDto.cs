using Questlog.Application.Common.DTOs.GuildMember.Response;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record CreateCampaignResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<GetMemberAvatarResponseDto> Members { get; set; }
    }
}