using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

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