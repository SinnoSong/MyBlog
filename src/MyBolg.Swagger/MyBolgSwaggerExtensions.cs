using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyBlog.Domain.Configurations;
using MyBlog.Domain.Shared;
using MyBolg.Swagger.Filters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyBolg.Swagger
{
    public static class MyBolgSwaggerExtensions
    {
        /// <summary>
        /// 当前Api版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = $"v{AppSettings.ApiVersion}";

        private static readonly string description = "接口分组描述";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "1.0.0",
                //    Title = "我的接口",
                //    Description = "接口描述"
                //});
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyBlog.HttpApi.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyBlog.Domain.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyBlog.Application.Contracts.xml"));
                options.DocumentFilter<SwaggerDocumentFilter>();
                // 开启JWT
                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入Bearer{Token}进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("oauth2", security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                // 遍历分组信息，生成Json
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/swagger/{x.UrlPrefix}/swagger.json", x.Name);
                });
                // 模型的默认扩展深度，位置为-1 完全隐藏
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = "博客-接口文档";
            });
        }

        #region SwaggerApiInfo

        internal class SwaggerApiInfo
        {
            /// <summary>
            /// Url前缀
            /// </summary>
            public string UrlPrefix { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///<see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }

        #endregion SwaggerApiInfo

        #region 初始化Swagger分组

        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>
        {
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v1,
                Name = "博客前台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version =version,
                    Title = "博客前台接口",
                    Description =description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v2,
                Name = "博客后台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version =version,
                    Title = "博客后台接口",
                    Description =description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v3,
                Name = "通用公共接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version =version,
                    Title = "通用公共接口",
                    Description =description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v4,
                Name = "JWT授权接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version =version,
                    Title = "JWT授权接口",
                    Description =description
                }
            }
    };

        #endregion 初始化Swagger分组
    }
}