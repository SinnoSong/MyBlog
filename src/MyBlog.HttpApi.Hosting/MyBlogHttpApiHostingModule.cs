using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Domain.Configurations;
using MyBlog.EntityFrameworkCore;
using MyBlog.HttpApi.Hosting.Filters;
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
                // 添加自己实体的MyBlogExceptionFilter
                options.Filters.Add(typeof(MyBlogExceptionFilter));
            });

            //路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
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
            app.UseHsts(); // 使用HSTS中间件添加严格传输安全头
            // 转发将表头代理到当前请求，配置Nginx使用，获取用户真是IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseRouting(); // 添加路由中间件
            app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());// 添加跨域
            // 异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // 身份认证
            app.UseAuthentication();
            // 认证授权
            app.UseAuthorization();

            app.UseHttpsRedirection(); // HTTP => HTTPS
            app.UseEndpoints(endpotions =>
            {
                endpotions.MapControllers();
            });
        }
    }
}