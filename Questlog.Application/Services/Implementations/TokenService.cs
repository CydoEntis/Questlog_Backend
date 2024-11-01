
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace Questlog.Application.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly string _jwtSecret;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _jwtSecret = configuration["JwtSecret"];
        _unitOfWork = unitOfWork;
    }

    public string CreateAccessToken(ApplicationUser user, string tokenId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "https://localhost:7265/",
            Audience = "http://localhost:5173/",
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenStr = tokenHandler.WriteToken(token);

        return tokenStr;
    }

    public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
    {
        var existingRefreshToken = await _unitOfWork.Token.GetRefreshToken(tokenDTO.RefreshToken);
        if (existingRefreshToken is null)
        {
            return new TokenDTO();
        }

        var isTokenValid = CheckIfTokenIsValid(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        if (!isTokenValid)
        {
            await InvalidateToken(existingRefreshToken);
            return new TokenDTO();
        }

        if (!existingRefreshToken.IsValid)
        {
            await _unitOfWork.Token.InvalidateAllUsersTokens(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        }


        if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
        {
            await InvalidateToken(existingRefreshToken);
            return new TokenDTO();
        }

        var newRefreshToken = await CreateRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

        await InvalidateToken(existingRefreshToken);

        var applicationUser = await _unitOfWork.User.GetUserById(existingRefreshToken.UserId);

        if (applicationUser is null)
        {
            return new TokenDTO();
        }

        var newAccessToken = CreateAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

        return new TokenDTO()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };
    }

    public async Task<string> CreateRefreshToken(string userId, string tokenId)
    {
        RefreshToken refreshToken = new()
        {
            IsValid = true,
            UserId = userId,
            JwtTokenId = tokenId,
            ExpiresAt = DateTime.UtcNow.AddHours(12),
            Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid()
        };

        await _unitOfWork.Token.AddRefreshToken(refreshToken);

        return refreshToken.Refresh_Token;

    }

    public async Task InvalidateToken(RefreshToken refreshToken)
    {
        refreshToken.IsValid = false;
        await _unitOfWork.SaveAsync();
        return;
    }

    public async Task RevokeRefreshToken(TokenDTO tokenDTO)
    {
        var existingRefreshToken = await _unitOfWork.Token.GetRefreshToken(tokenDTO.RefreshToken);

        if (existingRefreshToken is null)
        {
            return;
        }

        var isTokenValid = CheckIfTokenIsValid(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

        if (!isTokenValid)
        {
            return;
        }

        await _unitOfWork.Token.InvalidateAllUsersTokens(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
    }

    private bool CheckIfTokenIsValid(string accessToken, string expectedUserId, string expectedTokenId)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(accessToken);
            var jwtTokenId = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
            var userId = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            return userId == expectedUserId && jwtTokenId == expectedTokenId;
        }
        catch
        {
            return false;
        }
    }

}
