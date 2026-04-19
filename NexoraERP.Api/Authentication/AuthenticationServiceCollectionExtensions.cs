using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace NexoraERP.Api.Authentication;

/// <summary>JWT bearer wiring — replace signing key and enable issuer validation for production IdP integration.</summary>
public static class AuthenticationServiceCollectionExtensions
{
    public const string JwtSectionName = "Jwt";

    public static IServiceCollection AddNexoraJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection(JwtSectionName);
        var signingKey = jwt["SigningKey"];
        if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32)
        {
            signingKey = "DEVELOPMENT-ONLY-REPLACE-WITH-256-BIT-SECRET-KEY!!";
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = jwt.GetValue("RequireHttpsMetadata", true);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = jwt.GetValue("ValidateIssuer", false),
                    ValidIssuer = jwt["Issuer"],
                    ValidateAudience = jwt.GetValue("ValidateAudience", false),
                    ValidAudience = jwt["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        services.AddAuthorization();
        return services;
    }
}
