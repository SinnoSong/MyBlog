using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Blog;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyBlog.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class MyBlogDbContext : AbpDbContext<MyBlogDbContext>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<FriendLink> FriendLinks { get; set; }

        public MyBlogDbContext(DbContextOptions<MyBlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configure();
        }
    }
}