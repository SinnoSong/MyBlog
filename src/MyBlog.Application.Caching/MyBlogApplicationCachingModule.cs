using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Domain;
using MyBlog.Domain.Configurations;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace MyBlog.Application.Caching
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(MyBlogDomainModule)
        )]
    public class MyBlogApplicationCachingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = AppSettings.Caching.RedisConnectionString;
            });

            var csredis = new CSRedis.CSRedisClient(AppSettings.Caching.RedisConnectionString);
            RedisHelper.Initialization(csredis);

            context.Services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }
    }
}