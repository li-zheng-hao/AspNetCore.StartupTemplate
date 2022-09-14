# 快速开始

```
dotnet new --install AspNetCore.StartupTemplate
// 进入项目根目录
dotnet new aspnetcorestartuptemplate -n XXX   //  XXX为项目名
```

1. 进入项目后全局搜索`AspNetCore.StartupTemplate`字符串，全部替换成自己的项目名
2. 打开`appsettings.json`相关配置文件(`appsettings.XXX.json`)，配置自己的环境
3. 打开`Program.cs`启动文件，根据自己的需求，把不需要使用的服务注册代码给删除，删除服务注册代码后需要把对应的controller示例给删除,如果出现报错的话删除对应部分的代码即可
4. 启动项目，点击各个controller示例测试是否存在问题
5. 开始进行自己的业务开发

> 需要注意有些库最好是不要替换，否则改动很大，例如ORM最好用freesql，否则得把仓储、服务、控制器、实体等N个层全都改一遍，会比较麻烦

# 项目结构

```
- 解决方案
|
---- Core : 核心库，包含了AOP、DTM结合spring注解式事务的扩展类等
|
---- Auth ：身份认证相关
|
---- Configuration：全局配置，包含了消息队列的routingkey字典，appsetting配置文件映射类，http业务代码字典
|
---- CoreGeneratorConsole：代码生成器控制台项目，通过提供实体类名一件生成实体、仓储、服务、控制器代码
|
---- DbMigration：数据库版本管理，在项目启动的时候自动比较数据库表版本并进行升级或回滚
|
---- Filter：过滤器相关，如模型验证、全局异常、swagger文件上传
|
---- SnowFlake：雪花id库，支持自动回收和生成workid(需要redis支持)
|
---- Utility：工具类库，集成了一些通用的扩展方法或帮助类
|
---- Deployment：一些部署脚本，如redis的哨兵集群(dockerc-compose版)
|
---- Contract：DTO、接口协议类
|
---- CacheAsync：Canal的缓存同步库
|
---- Repository、IRepository：仓储层
|
---- Services、IServices：业务层
|
---- Webapi：控制器层
```

# Spring注解式事务

Spring的事务是通过在接口上加注解的方式来标记此方法内的CURD需要在事务中执行，在这种开发模式下，我们不用操心事务的开启、提交和回滚，并且还能利用事务的传播属性很方便的在调用其它接口方法时根据需求来选择是否需要开启新的事务或是加入当前事务。这种事务处理方式在.NET中是通过特性来实现的，因此这里也可以叫特性事务。

特性事务支持六种传播方式(propagation)，意味着跨方法的事务非常方便，并且支持同步异步：

- Requierd：如果当前没有事务，就新建一个事务，如果已存在一个事务中，加入到这个事务中，默认的选择。
- Supports：支持当前事务，如果没有当前事务，就以非事务方法执行。
- Mandatory：使用当前事务，如果没有当前事务，就抛出异常。
- NotSupported：以非事务方式执行操作，如果当前存在事务，就把当前事务挂起。
- Never：以非事务方式执行操作，如果当前事务存在则抛出异常。
- Nested：以嵌套事务方式执行（这里是开启新的事务，而不是和Spring的Nested一样）

这一块的实现是FreeSql自带的，因此我们只需要拿过来用就可以了。下面是示例：

```c#
[Transactional(Propagation = Propagation.Nested)]
public void TestNestedTransOk()
{
    
}
```



# 分布式事务

本项目集成了Dtm和CAP框架，两种都可以实现分布式事务，并且能够和FreeSql的特性事务融合在一起使用，无需自己手动提交和回滚，下面是不同场景下的使用选择：

1. 如果是微服务的情况下，存在A、B、C多个服务，A需要调B、C，且失败后需要回滚，这种情况要用Dtm，建议SAGA模式。如果不需要回滚，即最终都要成功，那CAP和DTM都可以。
2. 如果是多个项目，如本项目是.NET，其它项目是Java、Go等，或者都是.NET，但是只有自己的项目用Dtm，那么建议使用CAP，能够保证消息一定能发送到其它应用内

注意点：

1. 用Dtm不用考虑幂等、空悬挂等问题，因为有子事务屏障，而CAP没有，因此在使用CAP时可能会出现重复消费，需要开发人员在设计接口时考虑幂等的问题

# 缓存

缓存这块使用的是Redis，在controller级别的缓存使用了`AspNetCore.CacheOutput`库，可以配置浏览器缓存或以类名+方法名为key进行服务端的缓存，并且支持自定义过期时间。

考虑到该库仅仅能在controller上使用，因此项目中自行实现了一套基于AOP的缓存机制，其基本思路与CacheOutput库相似，使用`NeedCacheAttribute`特性标记查询方法进行缓存，同时使用`ClearCacheAttribute`特性标记增删改方法，在执行时会根据配置来清除对应方法的缓存，并且在缓存时处理了缓存雪崩（随机过期时间）、穿透（空值）、击穿（分布式锁）的问题。

由于以上两种方式的缓存更新不太好把握，因此后续会加入Canal进行表级别的同步，这样就不用考虑增删改导致的缓存失效问题了。

在实际业务开发过程中可以根据需求灵活使用上面两种缓存方式。

# 消息队列

消息队列底层使用RabbitMQ进行传输，但是我们不会直接去调用RabbitMQ的接口，而是使用CAP去处理消息，这是因为直接使用RabbitMQ需要保证消息的可靠性，还得封装一套发布和订阅的库，会比较麻烦，而这些CAP都给我们做好了，不过CAP封装的会比较抽象，因此灵活性不太够，下面有几点是需要注意的：

1. CAP设置ExchangeName不方便，在一些特定场景下如果要切换交换机，CAP是不支持的
2. 如果要和外系统交互（直接使用RabbitMQ的生产者），需要添加一些特定的cap头参数，否则无法正确订阅到对应的消息
3. 只有Topic模式，其它几种模式，像Direct、Fanout都没有

# 身份认证

todo

# 服务注册发现

todo

# 微服务

todo