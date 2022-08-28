namespace AspNetCore.StartUpTemplate.Auth;

/// <summary>
/// 用户信息payload
/// </summary>
public class TokenModel
{ 
  public DateTime ExpireTime { get; set; }
  public UserData UserData{ get; set; }
}

public class UserData
{
    public string Id { get; set; }
    public string UserName { get; set; }
}