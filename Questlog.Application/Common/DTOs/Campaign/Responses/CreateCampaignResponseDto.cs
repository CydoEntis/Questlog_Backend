using Questlog.Application.Common.DTOs.GuildMember.Response;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record CreateCampaignResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<GetMemberResponseDto> Members { get; set; }
    }
}