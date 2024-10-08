using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/guild")]
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

            return CreatedResponse(result.Data);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateGuild([FromBody] UpdateGuildRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("UpdateGuildRequestDTO cannot be null.");
            }

            var result = await _guildService.UpdateGuild(requestDTO);

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
