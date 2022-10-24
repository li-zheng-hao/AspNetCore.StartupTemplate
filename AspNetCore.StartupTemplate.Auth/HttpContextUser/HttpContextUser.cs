using Microsoft.AspNetCore.Http;

namespace AspNetCore.StartUpTemplate.Auth.HttpContextUser;

public class HttpContextUser:IHttpContextUser
{
    private readonly IHttpContextAccessor _accessor;
    
    public HttpContextUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        UserData= TokenHelper.ResolveToken(GetToken()).UserData;
    }

    public string? UserName => UserData?.UserName;
    public long? ID
    {
        get
        {
            bool  res=Int64.TryParse(UserData?.Id, out var id);
            if (res)
                return id;
            else
                return -1;
        }
    }

    public string? Role => UserData?.Role;
    public UserData? UserData { get; }
    public string GetToken()
    {
        return _accessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }
}