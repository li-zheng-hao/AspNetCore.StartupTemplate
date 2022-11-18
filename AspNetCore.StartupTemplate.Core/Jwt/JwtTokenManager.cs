using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quickwire.Attributes;

namespace AspNetCore.StartUpTemplate.Core.Jwt;

[RegisterService(ServiceLifetime.Singleton)]
[InjectAllInitOnlyProperties]
public class JwtTokenManager
{
    public JwtOption _jwtOption { get; init; }
    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public string GenerateToken(Claim[] claims)
    {
        // var claims = new[]
        // {
            // new Claim(ClaimTypes.Name, username),
            // new Claim(ClaimTypes.Role, "Admin"),
            // new Claim(ClaimTypes.Role, "User"),
            // new Claim(ClaimTypes.Role, "Guest"),
        // };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_jwtOption.ExpireSeconds),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    /// <summary>
    /// 解析token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public ClaimsPrincipal ResolveToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenValidationParameters = JwtHelper.GetTokenValidatonParameter(_jwtOption, false);
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        return principal;
    }
}