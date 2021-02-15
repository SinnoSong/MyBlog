using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace MyBlog.EntityFrameworkCore.DbMigrations
{
    [DependsOn(
        typeof(MyBlogFrameworkCoreModule)
        )]
    public class MyBlogEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyBlogMigrationsDbContext>();
        }
    }
}