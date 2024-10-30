using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.DTOs.Quest;

namespace Questlog.Application.Common.DTOs.Campaign.Responses
{
    public record GetCampaignResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string Creator { get; set; }
        public string Color { get; set; }
        public List<GetMemberResponseDto> Members { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DueDate { get; set; }

    }
}