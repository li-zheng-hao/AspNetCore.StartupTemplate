using AspNetCore.StartUpTemplate.Model;

namespace AspNetCore.StartUpTemplate.Contract.DTOs;

public class MapsterSampleDto:BaseDto<MapsterSampleDto,Users>
{
    public string CustomId { get; set; }
    public string CustomPassword { get; set; }
    public override void AddCustomMappings()
    {
        SetCustomMappings().TwoWays().Map(model => model.Id, it => it.CustomId)
            .Map(model => model.Password, it => it.CustomPassword);
        // 使用TwoWay就不需要再写反向映射
        // SetCustomMappingsInverse().Map(model => model.CustomId, it => it.Id)
            // .Map(model => model.CustomPassword, it => it.Password);
    }
}