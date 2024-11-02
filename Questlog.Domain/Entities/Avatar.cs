namespace Questlog.Domain.Entities;

public class Avatar
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Tier { get; set; }
    public int UnlockLevel { get; set; }
    public int Cost { get; set; }
}
