﻿namespace Questlog.Application.Common.DTOs.Avatar;

public class AvatarShopDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public int Tier { get; set; }
    public int UnlockLevel { get; set; }
    public int Cost { get; set; }
    public bool IsUnlocked { get; set; }
}