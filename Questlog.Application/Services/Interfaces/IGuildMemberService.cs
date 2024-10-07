using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.Models;
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
        Task<ServiceResult<CreateGuildMemberResponseDTO>> CreateGuildMember(CreateGuildMemberRequestDTO requestDTO);
        //Task<ServiceResult<GuildMember>> GetGuildMember(int guildId, string userId);
        //Task<ServiceResult<List<GuildMember>>> GetAllGuildMembers(int guildId);
        //Task<ServiceResult<GuildMember>> CreateGuildMember(GuildMember guildMember);
        //Task<ServiceResult<GuildMember>> UpdateGuildMember(GuildMember guildMember);
        //Task<ServiceResult<GuildMember>> RemoveGuildMember(GuildMember guildMember);
    }
}
