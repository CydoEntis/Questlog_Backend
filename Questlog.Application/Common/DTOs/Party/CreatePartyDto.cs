namespace Questlog.Application.Common.DTOs.Party;


public class CreatePartyDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DateTime DueDate { get; set; }
}