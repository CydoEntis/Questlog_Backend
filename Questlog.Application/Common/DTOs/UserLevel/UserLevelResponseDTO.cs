using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.UserLevel
{
    public record UserLevelResponseDTO
    {
        public int CurrentLevel { get; set; }
        public int CurrentExp { get; set; }
    }
}
