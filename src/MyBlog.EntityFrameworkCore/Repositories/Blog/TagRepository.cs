using MyBlog.Domain.Blog;
using MyBlog.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyBlog.EntityFrameworkCore.Repositories.Blog
{
    /// <summary>
    /// TagRepository
    /// </summary>
    public class TagRepository : EfCoreRepository<MyBlogDbContext, Tag, int>, ITagRepository
    {
        public TagRepository(IDbContextProvider<MyBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<Tag> tags)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Set<Tag>().AddRangeAsync(tags);
            await dbContext.SaveChangesAsync();
        }
    }
}