using Questlog.Application.Common.DTOs.Avatar;

namespace Questlog.Application.Common.DTOs.Member;

public class MemberDto
{
    public int Id { get; set; }
    public int PartyId { get; set; }
    public string Role { get; set; }
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public AvatarDto Avatar { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime JoinedOn { get; set; }
}