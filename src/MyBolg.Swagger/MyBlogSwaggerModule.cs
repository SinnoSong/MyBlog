using Microsoft.AspNetCore.Builder;
using MyBlog.Domain;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MyBolg.Swagger
{
    [DependsOn(
        typeof(MyBlogDomainModule)
        )]
    public class MyBlogSwaggerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwagger();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.GetApplicationBuilder().UseSwagger().UseSwaggerUI();
        }
    }
}