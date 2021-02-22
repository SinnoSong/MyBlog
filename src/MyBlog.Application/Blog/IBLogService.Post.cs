using MyBlog.Blog;

using MyBolg.ToolKits.Base;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog
{
    public partial interface IBlogService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input);
    }
}