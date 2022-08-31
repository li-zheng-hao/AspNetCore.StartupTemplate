# 语言

- English [Readme-En](./README-EN.md)
- 中文 [ReadMe-中文](./README.md)

# 介绍

 Asp.Net Core Webapi 项目快速启动模板

# 项目结构

- `Congfiguration`:项目配置库
- `Core`:核心公共基础库
- `Utility`:工具类库
- `Filter`:过滤器库
- `Models`:实体层
- `Repository`,`IRepository`:仓储层
- `IServices`,`Services`:服务层
- `Webapi`: Webapi层
- `MQ`:rabbitmq官方库封装
- `Redis`:redis库封装
- `Snowflake`:支持workid自动获取回收的雪花id库
- `Logging`:Serilog日志库配置，支持输出至文件，控制台及ElasticSearch

# 技术栈

- 序列化：`Newtonsoft.Json`
- 身份验证：`Jwt.Net`
- 数据库ORM：`SqlSugar`
- 接口缓存：`CacheOutput`
- 实体映射：`AutoMapper`
- 消息队列：`RabbitMQ`

# 致谢

1. [https://github.com/CoreUnion/CoreShop](https://github.com/CoreUnion/CoreShop) .Net Core 商城APP
2. [https://www.donet5.com/Home/Doc](https://www.donet5.com/Home/Doc) SqlSugar 非常好用的ORM
3. [https://github.com/fuluteam/ICH.Snowflake](https://github.com/fuluteam/ICH.Snowflake) 项目中使用的雪花ID生成器
4. [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/) Rider .Net IDE
5. [https://github.com/luoyunchong/lin-cms-dotnetcore](https://github.com/luoyunchong/lin-cms-dotnetcore) 一个前后端分离的 CMS 开源项目
6. [https://freesql.net/guide/](https://freesql.net/guide/) FreeSql 非常好用的ORM

# 开源协议

[MIT license](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/LICENSE)