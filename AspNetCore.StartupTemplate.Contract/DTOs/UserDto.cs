using AspNetCore.StartUpTemplate.Model;

namespace AspNetCore.StartUpTemplate.Contract.DTOs;

public class UserDto : BaseDto<UserDto,Users>
{
    
    public string Id { get; set; }
   
    public string UserName { get; set; }
    
    public string Password { get; set; }


    public override void AddCustomMappings()
    {
        SetCustomMappings()
            .Map(dest => dest.UserName,
                src => src.UserName)
            .Map(dest => dest.Password, src => src.Password).TwoWays();
        // SetCustomMappingsInverse()
            // .Map(dest => dest.UserName, src => src.UserName);
    }
}