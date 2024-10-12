using Questlog.Application.Common.DTOs.ApplicationUser.Response;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Responses
{
    public record CreateGuildMemberResponseDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GuildId { get; set; }
        public string Role { get; set; }
        public DateTime JoinedOn { get; set; }
        public GetApplicationUserResponseDTO ApplicationUser { get; set; }
    }
}
