using AutoMapper;

namespace AspNetCore.StartUpTemplate.Mapping;

public class AutoMapperConfig:Profile
{
    public AutoMapperConfig()
    {
        //CreateMap<Manager, ManagerDTO>().ReverseMap();
        //CreateMap<Manager, ManagerDTO>().AfterMap((from,to,context)=>{});

    }
           

}