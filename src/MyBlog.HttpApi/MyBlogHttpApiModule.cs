using MyBlog.Application;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.HttpApi
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(MyBlogApplicationModule)
        )]
    public class MyBlogHttpApiModule : AbpModule
    {
    }
}