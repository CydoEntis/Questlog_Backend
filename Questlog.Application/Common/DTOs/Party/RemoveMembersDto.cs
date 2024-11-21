namespace Questlog.Application.Common.DTOs.Party;

public class RemoveMembersDto
{
    public int PartyId { get; set; }
    public int[] MemberIds { get; set; }
}