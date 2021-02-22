using MyBlog.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Caching.Blog.Impl
{
    public partial class BlogCacheService : IBlogCacheService
    {
        private const string KEY_QueryPosts = "Blog:Post:Query-{0}-{1}";

        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input, Func<Task<ServiceResult<PagedList<QueryPostDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_QueryPosts.FormatWith(input.Page, input.Limit), factory, CacheStrategy.ONE_DAY);
        }
    }
}