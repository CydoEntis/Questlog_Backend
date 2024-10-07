using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IGuildMemberService
    {
        Task<GuildMember> GetGuildMember(int guildId, string userId);
        Task<GuildMember> CreateGuildMember(GuildMember guildMember);
    }
}
