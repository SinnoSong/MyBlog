using MyBlog.Blog;
using MyBolg.ToolKits.Base;
using System;
using System.Threading.Tasks;

namespace MyBlog.Application.Caching.Blog
{
    public partial interface IBlogCacheService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input, Func<Task<ServiceResult<PagedList<QueryPostDto>>>> factory);
    }
}