using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.DbMigration;

public static class DbMigrateDependencyInjection
{
    public static IServiceCollection AddDbMigration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<DbMigation>();
        return serviceCollection;
    }
    /// <summary>
    /// 更新db
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void MigrateDatabase(this IServiceProvider serviceProvider)
    {
        serviceProvider.GetService<DbMigation>()?.Migrate();
    }
}