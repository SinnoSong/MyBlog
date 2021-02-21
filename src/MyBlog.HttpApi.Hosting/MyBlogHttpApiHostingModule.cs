using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Domain.Configurations;
using MyBlog.EntityFrameworkCore;
using MyBlog.HttpApi.Hosting.Middleware;
using MyBolg.BackgroundJobs;
using MyBolg.Swagger;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyBlog.HttpApi.Hosting
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(MyBlogHttpApiModule),
        typeof(MyBlogSwaggerModule),
        typeof(MyBlogFrameworkCoreModule),
        typeof(MyBlogBackgroundJobsModule)
    )]
    public class MyBlogHttpApiHostingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            Configure<MvcOptions>(options =>
            {
                var filterMetadate = options.Filters.FirstOrDefault(x => x is ServiceFilterAttribute attribute && attribute.ServiceType.Equals(typeof(AbpExceptionFilter)));

                // 移除AbpExceptionFilter
                options.Filters.Remove(filterMetadate);
            });
            // 身份验证
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,
                        ValidAudience = AppSettings.JWT.Domain,
                        ValidIssuer = AppSettings.JWT.Domain,
                        IssuerSigningKey = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.GetBytes())
                    };
                });
            // 添加授权
            context.Services.AddAuthorization();
            // Http请求
            context.Services.AddHttpClient();
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
            // 异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // 身份认证
            app.UseAuthentication();
            // 认证授权
            app.UseAuthorization();
            app.UseEndpoints(endpotions =>
            {
                endpotions.MapControllers();
            });
        }
    }
}