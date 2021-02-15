using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Domain.Shared
{
    [DependsOn(
        typeof(AbpIdentityDomainModule)
        )]
    public class MyBlogDomainSharedModule : AbpModule
    {
    }
}