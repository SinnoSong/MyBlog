﻿using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyBlog.Application
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule)
        )]
    public class MyBlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}