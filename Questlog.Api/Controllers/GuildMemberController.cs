using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Questlog.Api.Controllers
{
    [Route("api/guild-member")]
    [Authorize]
    [ApiController]
    public class GuildMemberController : ControllerBase
    {
    }
}
