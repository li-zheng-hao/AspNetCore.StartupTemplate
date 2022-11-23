namespace AspNetCore.StartupTemplate.IntegrationTest.Core;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 在Staging环境进行自动化集成测试 或者单独弄个SUT环境配置搞都行
        // 这里图方便用开发环境
        builder.UseEnvironment(Environments.Development);
        //the test app's builder.ConfigureServices callback is executed before the SUT's Startup.ConfigureServices code.
        builder.ConfigureServices(services =>
        {
            // TODO 在这里初始化种子数据
            
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbContextOptions<ApplicationDbContext>));
            //
            // services.Remove(descriptor);
            //
            // services.AddDbContext<ApplicationDbContext>(options =>
            // {
            //     options.UseInMemoryDatabase("InMemoryDbForTesting");
            // });
            //
            // var sp = services.BuildServiceProvider();
            //
            // using (var scope = sp.CreateScope())
            // {
            //     var scopedServices = scope.ServiceProvider;
            //     var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            //     var logger = scopedServices
            //         .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
            //
            //     db.Database.EnsureCreated();
            //
            //     try
            //     {
            //         Utilities.InitializeDbForTests(db);
            //     }
            //     catch (Exception ex)
            //     {
            //         logger.LogError(ex, "An error occurred seeding the " +
            //                             "database with test messages. Error: {Message}", ex.Message);
            //     }
            // }
        });
        //The test app's builder.ConfigureTestServices callback is executed after.
        builder.ConfigureTestServices(services =>
        {

        });
    }
}