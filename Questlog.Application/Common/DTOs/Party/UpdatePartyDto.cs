namespace Questlog.Application.Common.DTOs.Party;


public class UpdatePartyDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public DateTime? DueDate { get; set; }
}