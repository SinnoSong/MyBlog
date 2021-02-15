using MyBlog.Domain;
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
            base.ConfigureServices(context);
        }
    }
}