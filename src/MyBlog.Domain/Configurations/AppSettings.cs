using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyBlog.Domain.Configurations
{
    /// <summary>
    /// appsetting.json配置文件读取
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 配置文件根节点
        /// </summary>
        private static readonly IConfigurationRoot _config;

        static AppSettings()
        {
            // 静态构造函数 加载appsetting并构建IConfigurationRoot
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsetting.json", true, true);
            _config = builder.Build();
        }

        /// <summary>
        /// 开启的Db key
        /// </summary>
        public static string EnableDb => _config["ConnectionStrings:Enable"];

        /// <summary>
        /// 根据key获取的链接字符串
        /// </summary>
        public static string ConnectionStrings => _config.GetConnectionString(EnableDb);
    }
}