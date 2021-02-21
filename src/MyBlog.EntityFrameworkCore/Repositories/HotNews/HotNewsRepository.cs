using MyBlog.Domain.HotNews.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyBlog.EntityFrameworkCore.Repositories.HotNews
{
    public class HotNewsRepository : EfCoreRepository<MyBlogDbContext, Domain.HotNews.HotNews, Guid>, IHotNewsRepository
    {
        public HotNewsRepository(IDbContextProvider<MyBlogDbContext> dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="hotNews"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<Domain.HotNews.HotNews> hotNews)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Set<Domain.HotNews.HotNews>().AddRangeAsync(hotNews);
            await dbContext.SaveChangesAsync();
        }
    }
}