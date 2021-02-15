using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyBlog.EntityFrameworkCore.DbMigrations
{
    public class MyBlogMigrationsDbContextFactory : IDesignTimeDbContextFactory<MyBlogMigrationsDbContext>
    {
        public MyBlogMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguation();
            var builder = new DbContextOptionsBuilder<MyBlogMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));
            return new MyBlogMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguation()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}