using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Application.Contracts.Blog.Params;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyBlog.HttpApi.Controllers
{
    public partial class BlogController
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet, Authorize, Route("admin/posts")]
        [ApiExplorerSettings(GroupName = Grouping.GroupName_v2)]
        public async Task<ServiceResult<PagedList<QueryPostForAdminDto>>> QueryPostsForAdminAsync([FromQuery] PaginInput input)
        {
            return await _blogService.QueryPostsForAdminAsync(input);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("post"), Authorize]
        [ApiExplorerSettings(GroupName = Grouping.GroupName_v2)]
        public async Task<ServiceResult> InsertPostAsync([FromBody] EditPostInput input)
        {
            return await _blogService.InsertPostAsync(input);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("post"), Authorize]
        [ApiExplorerSettings(GroupName = Grouping.GroupName_v2)]
        public async Task<ServiceResult> UpdatePostAsync([Required] int id, [FromBody] EditPostInput input)
        {
            return await _blogService.UpdatePostAsync(id, input);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("post"), Authorize]
        [ApiExplorerSettings(GroupName = Grouping.GroupName_v2)]
        public async Task<ServiceResult> DeletePostAsync([Required] int id)
        {
            return await _blogService.DeletePostAsync(id);
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("admin/post"), Authorize]
        [ApiExplorerSettings(GroupName = Grouping.GroupName_v2)]
        public async Task<ServiceResult<PostForAdminDto>> GetPostForAdminAsync([Required] int id)
        {
            return await _blogService.GetPostForAdminAsync(id);
        }
    }
}