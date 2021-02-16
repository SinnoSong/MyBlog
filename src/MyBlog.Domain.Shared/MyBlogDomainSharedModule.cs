using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Domain.Shared
{
    [DependsOn(
        typeof(AbpIdentityDomainSharedModule)
        )]
    public class MyBlogDomainSharedModule : AbpModule
    {
    }
}