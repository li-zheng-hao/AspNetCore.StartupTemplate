# 语言

- English [Readme-En](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/README-EN.md)
- 中文 [ReadMe-中文](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/README.md)

# 介绍

Asp.Net Core快速启动项目模板,支持微服务/单体应用,以下功能开箱即用：

1. Spring式注解事务
2. Spring式依赖注入
3. 工作单元+仓储
4. 消息队列
5. API接口缓存
6. 代码模板生成器
7. 数据库迁移版本管理
8. 分布式雪花id
9. 服务注册发现
10. 分布式事务
11. 身份认证
12. 定时任务
13. 更多...

详细文档后续待发布...

各个部分都有示例代码，根据模板新建项目后可根据相关功能的示例代码参考开发

欢迎任何star、issue、pr~⭐

# 开发环境

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

> 也可以直接新建项目，然后把这个项目当做一个素材库，用到什么就复制什么

# 项目结构

- `Congfiguration`:项目配置库
- `Core`:核心公共基础库
- `Utility`:工具类库
- `Filter`:过滤器库
- `Caching`:缓存库
- `Models`:实体层
- `Repository`,`IRepository`:仓储层
- `IServices`,`Services`:服务层
- `Webapi`: Webapi层
- `Snowflake`:支持workid自动获取回收的雪花id库
- `DbMigration`:数据库版本迁移库
- `Job`：定时任务
- `CodeGeneratorConsole`:代码生成器
- `UnitTest`:单元测试
- `IntergrationTest`:集成测试

# 技术栈

- 序列化：`Newtonsoft.Json`
- 身份验证：`Jwt.Net`
- 数据库ORM：`FreeSql`
- 接口缓存：`CacheOutput`,自己简单封装的库
- 实体映射：`Mapster`
- 消息队列：`RabbitMQ`
- 事件总线: `CAP`
- 分布式事务：`DTM`+`CAP`
- 数据迁移：`Evolve`
- Redis:`FreeRedis`
- 编译时AOP：`Rougamo`
- 单元测试：`XUnit`
- 模板生成：`DotLiquid`
- Http库：`Flurl.Http`
- Mock库：`Moq`
- 定时任务：`Hangfire`

# 致谢

1. [https://github.com/CoreUnion/CoreShop](https://github.com/CoreUnion/CoreShop) .Net Core 商城APP
2. [https://www.donet5.com/Home/Doc](https://www.donet5.com/Home/Doc) SqlSugar 非常好用的ORM
3. [https://github.com/fuluteam/ICH.Snowflake](https://github.com/fuluteam/ICH.Snowflake) 项目中使用的雪花ID生成器
4. [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/) Rider .Net IDE
5. [https://github.com/luoyunchong/lin-cms-dotnetcore](https://github.com/luoyunchong/lin-cms-dotnetcore) 一个前后端分离的 CMS 开源项目
6. [https://freesql.net/guide/](https://freesql.net/guide/) FreeSql 非常好用的ORM
7. [https://github.com/lecaillon/Evolve](https://github.com/lecaillon/Evolve) 数据库版本控制库
8. [https://github.com/inversionhourglass/Rougamo](https://github.com/inversionhourglass/Rougamo) 编译时AOP框架
9. 其它...

# 开源协议

[MIT license](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/LICENSE)
