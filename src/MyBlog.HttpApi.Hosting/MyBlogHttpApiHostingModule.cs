using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MyBlog.EntityFrameworkCore;
using MyBolg.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyBlog.HttpApi.Hosting
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(MyBlogHttpApiModule),
        typeof(MyBlogSwaggerModule),
        typeof(MyBlogFrameworkCoreModule)
    )]
    public class MyBlogHttpApiHostingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // 生成开发者异常页面
            }
            app.UseRouting(); // 添加路由中间件
            app.UseEndpoints(endpotions =>
            {
                endpotions.MapControllers();
            });
        }
    }
}