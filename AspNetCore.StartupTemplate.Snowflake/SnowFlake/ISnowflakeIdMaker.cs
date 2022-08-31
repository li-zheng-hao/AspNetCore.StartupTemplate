namespace AspNetCore.StartupTemplate.Snowflake.SnowFlake
{
    public interface ISnowflakeIdMaker
    {
        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        long NextId(int? workId = null);
    }
}