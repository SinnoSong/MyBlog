using MyBlog.Domain.Shared;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Domain
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(MyBlogDomainSharedModule)
    )]
    public class MyBlogDomainModule : AbpModule
    {
    }
}