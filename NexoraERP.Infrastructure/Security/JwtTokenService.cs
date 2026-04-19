using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NexoraERP.Application.Abstractions;

namespace NexoraERP.Infrastructure.Security;

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    private const string TenantIdClaim = "tenant_id";

    public JwtAccessTokenResult CreateAccessToken(Guid userId, string userName, Guid tenantId)
    {
        var jwt = configuration.GetSection("Jwt");
        var signingKey = jwt["SigningKey"];
        if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32)
            signingKey = "DEVELOPMENT-ONLY-REPLACE-WITH-256-BIT-SECRET-KEY!!";

        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var minutes = jwt.GetValue("AccessTokenExpirationMinutes", 60);
        if (minutes < 1) minutes = 60;

        var expires = DateTime.UtcNow.AddMinutes(minutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userName),
            new(TenantIdClaim, tenantId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(issuer))
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
        if (!string.IsNullOrEmpty(audience))
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));

        var token = new JwtSecurityToken(
            issuer: string.IsNullOrEmpty(issuer) ? null : issuer,
            audience: string.IsNullOrEmpty(audience) ? null : audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.WriteToken(token);
        var expiresIn = Math.Max(1, (int)(token.ValidTo - DateTime.UtcNow).TotalSeconds);

        return new JwtAccessTokenResult(accessToken, new DateTimeOffset(token.ValidTo, TimeSpan.Zero), expiresIn);
    }
}
