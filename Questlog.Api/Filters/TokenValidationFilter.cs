using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

public class TokenValidationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(token);
        var userId = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["UserId"] = userId;
    }
}
