# Language

- English [Readme-En](./README-EN.md)
- 中文 [ReadMe-中文](./README.md)

# Introduction

 Asp.Net Core Webapi project quick start template

# Project Structure

- `Congfiguration`:Project configuration library
- `Core`:Core common infrastructure library
- `Utility`:Tool library
- `Filter`:Filter library
- `Models`:Entity layer
- `Repository`,`IRepository`:Eepository layer
- `IServices`,`Services`:Service layer
- `Webapi`: Webapi layer
- `MQ`:Official RabbitMQ library encapsulation
- `Redis`:Redis library encapsulation
- `Snowflake`:Support workID to automatically obtain the reclaimed snowflake ID library
- `Logging`:Serilog log library configuration, support output to file, console and ElasticSearch

# Technology Stack

- Serialization：`Newtonsoft.Json`
- Authentication：`Jwt.Net`
- Database ORM：`SqlSugar`
- Interface cache：`CacheOutput`
- entity mapper：`AutoMapper`
- message queue：`RabbitMQ`

# Acknowledgements

1. [https://github.com/CoreUnion/CoreShop](https://github.com/CoreUnion/CoreShop) .Net Core shop APP
2. [https://www.donet5.com/Home/Doc](https://www.donet5.com/Home/Doc) SqlSugar very useful ORM
3. [https://github.com/fuluteam/ICH.Snowflake](https://github.com/fuluteam/ICH.Snowflake) The snowflake ID generator used in the project
4. [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/) Rider .Net IDE

# Open Source Licenses

[MIT license](https://github.com/li-zheng-hao/AspNetCore.StartupTemplate/blob/main/LICENSE)