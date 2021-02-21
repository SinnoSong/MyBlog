using MyBlog.Application.Caching;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Application
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule),
        typeof(MyBlogApplicationCachingModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MyBlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MyBlogApplicationModule>(validate: true);
                options.AddProfile<MyBlogAutoMapperProfile>(validate: true);
            });
        }
    }
}