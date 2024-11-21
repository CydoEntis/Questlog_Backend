namespace Questlog.Application.Common.DTOs.Member;

public class ChangeCreatorDto
{
    public int PartyId { get; set; }
    public int NewCreatorId { get; set; }
    public int OldCreatorId { get; set; }
    public string OldCreatorRole { get; set; }
}