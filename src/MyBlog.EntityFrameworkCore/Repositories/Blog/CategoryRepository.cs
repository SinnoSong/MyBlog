using MyBlog.Domain.Blog;
using MyBlog.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyBlog.EntityFrameworkCore.Repositories.Blog
{
    /// <summary>
    /// CategoryRepository
    /// </summary>
    public class CategoryRepository : EfCoreRepository<MyBlogDbContext, Category, int>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider<MyBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}