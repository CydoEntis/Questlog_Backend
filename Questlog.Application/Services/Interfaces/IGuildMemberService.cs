using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Response;
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
        Task<ServiceResult<GetGuildMemberResponseDTO>> GetGuildMember(int guildId, int guildMemberId);
        Task<ServiceResult<List<GetGuildMemberResponseDTO>>> GetAllGuildMembers(int guildId);
        Task<ServiceResult<GuildMemberResponseDTO>> CreateGuildMember(CreateGuildMemberRequestDTO requestDTO);
        Task<ServiceResult<GuildMemberResponseDTO>> UpdateGuildMember(UpdateGuildMemberRequestDTO requestDTO);
        Task<ServiceResult<int>> RemoveGuildMember(int guildId, int guildMemberId);
    }
}
