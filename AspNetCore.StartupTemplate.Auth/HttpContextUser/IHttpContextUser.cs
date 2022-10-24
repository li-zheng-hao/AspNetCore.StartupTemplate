namespace AspNetCore.StartUpTemplate.Auth.HttpContextUser;

/// <summary>
/// 登录后的用户信息
/// </summary>
public interface IHttpContextUser
{
    string? UserName { get; }
    long? ID { get; }
    string? Role { get;  }
    UserData? UserData { get; }
    string? GetToken();
    
}