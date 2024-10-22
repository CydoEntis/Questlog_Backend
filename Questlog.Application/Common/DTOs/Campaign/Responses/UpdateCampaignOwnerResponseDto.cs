using Questlog.Application.Common.DTOs.Member.Response;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record UpdateCampaignOwnerResponseDto
    {
        public GetMemberResponseDto OldLeader { get; set; }
        public GetMemberResponseDto NewLeader { get; set; }
        public int GuildId { get; set; }
    }
}
