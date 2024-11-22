﻿namespace Questlog.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITokenRepository Token { get; }
    IPartyRepository Party { get; }
    IMemberRepository Member { get; }
    IMemberQuestRepository MemberQuest { get; }
    IQuestRepository Quest { get; }
    IStepRepository Step { get; }
    IInviteTokenRepository InviteToken { get; }
    IAvatarRepository Avatar { get; }
    IUnlockedAvatarRepository UnlockedAvatar { get; }
    Task SaveAsync();
}