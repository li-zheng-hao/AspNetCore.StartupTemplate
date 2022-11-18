using System.Text;
using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.StartUpTemplate.Core.Jwt;

public static class JwtHelper
{
    public static TokenValidationParameters GetTokenValidatonParameter(JwtOption jwtOption,bool validateLifetime=true)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return  new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = creds.Key,
            ValidateIssuer = true,
            ValidIssuer = jwtOption.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOption.Audience,
            // 仅仅作为解析token的时候使用 不会验证token的过期时间
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
        };
    }
}