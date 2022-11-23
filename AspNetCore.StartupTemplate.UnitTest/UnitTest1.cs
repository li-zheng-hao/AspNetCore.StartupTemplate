using System.Reflection;
using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartUpTemplate.Contract.DTOs;
using AspNetCore.StartUpTemplate.Model;
using Mapster;

namespace AspNetCore.StartupTemplate.UnitTest;

public class UnitTest1
{
    [Fact]
    public void MapsterTest()
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Apply(new MapsterSampleDto());
        Users user = new Users();
        user.Id = 123456;
        user.Password = "123456";
        var mapsterSampleDto = MapsterSampleDto.FromEntity(user);
        Assert.Equal(user.Id.ToString(), mapsterSampleDto.CustomId);
        Assert.Equal(user.Password, mapsterSampleDto.CustomPassword);
    }
}