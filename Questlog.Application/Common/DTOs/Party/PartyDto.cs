using Questlog.Application.Common.DTOs.Member;

namespace Questlog.Application.Common.DTOs.Party;

public class PartyDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatorId { get; set; }
    public string Creator { get; set; }
    public string Color { get; set; }
    public List<MemberDto> Members { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DueDate { get; set; }
}