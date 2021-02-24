using MyBlog.Application.Contracts.Blog;
using MyBlog.Application.Contracts.Blog.Params;
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
        Task<ServiceResult<PagedList<QueryPostForAdminDto>>> QueryPostsForAdminAsync(PaginInput input);

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ServiceResult> InsertPostAsync(EditPostInput input);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ServiceResult> UpdatePostAsync(int id, EditPostInput input);

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult> DeletePostAsync(int id);

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<PostForAdminDto>> GetPostForAdminAsync(int id);
    }
}