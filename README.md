# 语言

- English [Readme-En](./README-EN.md)
- 中文 [ReadMe-中文](./README.md)

# 介绍

  Asp.Net Core快速启动项目模板,支持微服务/单体应用,以下功能开箱即用：

1. Spring式注解事务
2. 工作单元+仓储
3. 消息队列
4. API接口缓存
5. 代码模板生成器
6. 数据库迁移版本管理
7. 分布式雪花id(支持自动回收获取)
8. Nacos服务注册中心
9. DTM分布式事务(SAGA模式)
10. 身份认证等功能
11. 定时任务

详细使用文档在Github上查看：[https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/wiki](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/wiki)

各个部分在Controller层都有示例代码，根据模板新建项目后可根据相关功能的示例代码参考开发

欢迎任何star、issue、pr~⭐

#  开发环境

- Rider 2022 + Visual Studio Community 2022
- .NET 6

# 快速开始

```sh
dotnet new --install AspNetCore.StartupTemplate
dotnet new aspnetcorestartuptemplate -n XXX   //  XXX为项目名
```

|                            | nuget                                                        |
| -------------------------- | ------------------------------------------------------------ |
| AspNetCore.StartupTemplate | [![Nuget](https://img.shields.io/nuget/dt/AspNetCore.StartupTemplate)](https://www.nuget.org/packages/AspNetCore.StartupTemplate) |

更详细的文档：[Wiki](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/wiki)

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

# 技术栈

- 序列化：`Newtonsoft.Json`
- 身份验证：`Jwt.Net`
- 数据库ORM：`FreeSql`
- 接口缓存：`CacheOutput`
- 实体映射：`AutoMapper`
- 消息队列：`RabbitMQ`
- 数据迁移：`Evolve`
- 缓存:`StackExchange.Redis`
- 编译时AOP：`Rougamo`
- 单元测试：`XUnit`
- 模板生成：`DotLiquid`
- Http库：`Flurl.Http`
- Mock库：`Moq`

# 致谢

1. [https://github.com/CoreUnion/CoreShop](https://github.com/CoreUnion/CoreShop) .Net Core 商城APP
2. [https://www.donet5.com/Home/Doc](https://www.donet5.com/Home/Doc) SqlSugar 非常好用的ORM
3. [https://github.com/fuluteam/ICH.Snowflake](https://github.com/fuluteam/ICH.Snowflake) 项目中使用的雪花ID生成器
4. [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/) Rider .Net IDE
5. [https://github.com/luoyunchong/lin-cms-dotnetcore](https://github.com/luoyunchong/lin-cms-dotnetcore) 一个前后端分离的 CMS 开源项目
6. [https://freesql.net/guide/](https://freesql.net/guide/) FreeSql 非常好用的ORM
7. [https://github.com/lecaillon/Evolve](https://github.com/lecaillon/Evolve) 数据库版本控制库
8. [https://github.com/inversionhourglass/Rougamo](https://github.com/inversionhourglass/Rougamo) 编译时AOP框架

# 开源协议

[MIT license](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/LICENSE)