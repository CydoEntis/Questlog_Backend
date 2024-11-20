namespace Questlog.Application.Common.DTOs.Member;

public class NewMemberRoleDto
{
    public int PartyId { get; set; }
    public List<MemberRoleDto> MemberRoles { get; set; }
}