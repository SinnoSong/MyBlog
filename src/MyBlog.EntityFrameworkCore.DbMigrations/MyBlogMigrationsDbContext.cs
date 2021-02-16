using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyBlog.EntityFrameworkCore.DbMigrations
{
    public class MyBlogMigrationsDbContext : AbpDbContext<MyBlogMigrationsDbContext>
    {
        public MyBlogMigrationsDbContext(DbContextOptions<MyBlogMigrationsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configure();
        }
    }
}