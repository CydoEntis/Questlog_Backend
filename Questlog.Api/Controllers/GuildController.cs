using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Services.Interfaces;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/guilds")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class GuildController : BaseController
    {
        protected ApiResponse _response;
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _response = new ApiResponse();
            _guildService = guildService;
        }

        [HttpGet("{guildId}")]
        public async Task<ActionResult<ApiResponse>> GetGuild(int guildId)
        {
            if (guildId <= 0) return BadRequestResponse("Guild Id must be provided.");

            var result = await _guildService.GetGuildById(guildId);

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllGuilds()
        {
            var result = await _guildService.GetAllGuilds();

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateGuild([FromBody] CreateGuildRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("CreateGuildRequestDTO cannot be null.");
            }

            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequestResponse("User Id is missing.");
            }

            var result = await _guildService.CreateGuild(userId, requestDTO);

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            var createdGuildId = result.Data.Id; // Assuming Id is a property of GuildResponseDTO
            var locationUri = Url.Action("GetGuild", "Guild", new { guildId = createdGuildId }, HttpContext.Request.Scheme);

            return CreatedResponse(new { Id = createdGuildId, Location = locationUri, Message = "Guild created successfully" });
        }

        [HttpPut("{guildId}/details")]
        public async Task<ActionResult<ApiResponse>> UpdateGuildDetails(int guildId, [FromBody] UpdateGuildDetailsRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("UpdateGuildRequestDTO cannot be null.");
            }

            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequestResponse("User Id is missing.");
            }

            var result = await _guildService.UpdateGuildDetails(requestDTO, userId);

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpPut("{guildId}/leader")]
        public async Task<ActionResult<ApiResponse>> UpdateGuildLeader(int guildId, [FromBody] UpdateGuildLeaderRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("UpdateGuildRequestDTO cannot be null.");
            }

            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequestResponse("User Id is missing.");
            }

            var result = await _guildService.UpdateGuildLeader(requestDTO, userId);

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpDelete("{guildId}")]
        public async Task<ActionResult<ApiResponse>> DeleteGuild(int guildId)
        {
            if (guildId <= 0) return BadRequestResponse("Guild Id must be provided.");

            var result = await _guildService.DeleteGuild(guildId);

            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }
    }
}
