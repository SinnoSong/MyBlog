using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Domain
{
    [DependsOn(
        typeof(AbpIdentityDomainModule)
    )]
    public class MyBlogDomainModule : AbpModule
    {
    }
}