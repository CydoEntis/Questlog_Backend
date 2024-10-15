using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using System.Net;

namespace Questlog.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult<ApiResponse> OkResponse(object result)
    {
        var response = new ApiResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = result
        };
        return Ok(response);
    }

    protected ActionResult<ApiResponse> BadRequestResponse(string message)
    {
        var response = new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            IsSuccess = false,
            Errors = new Dictionary<string, List<string>> { { "badRequest", new List<string> { message } } }
        };
        return BadRequest(response);
    }

    protected ActionResult<ApiResponse> CreatedResponse(object result)
    {
        var response = new ApiResponse
        {
            StatusCode = HttpStatusCode.Created,
            IsSuccess = true,
            Result = result
        };
        return Ok(response);
    }

    protected ActionResult<ApiResponse> InternalServerErrorResponse(string message)
    {
        var response = new ApiResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            IsSuccess = false,
            Errors = new Dictionary<string, List<string>> { { "serverError", new List<string> { message } } }
        };
        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }
}
