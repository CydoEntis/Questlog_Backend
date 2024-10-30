
using Questlog.Application.Common.DTOs.Member.Response;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record CreateCampaignResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<GetMemberAvatarResponseDto> Members { get; set; }
    }
}