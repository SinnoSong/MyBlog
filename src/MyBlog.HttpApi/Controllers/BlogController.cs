using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Blog;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MyBlog.HttpApi.Controllers
{
    [ApiController, ApiExplorerSettings(GroupName = Grouping.GroupName_v1)]
    [Route("[controller]"), Authorize]
    public partial class BlogController : AbpController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region Posts

        /// <summary>
        /// 分页查询文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet, Route("posts")]
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync([FromQuery] PaginInput input)
        {
            return await _blogService.QueryPostAsync(input);
        }

        /// <summary>
        /// 根据Url查询文章详情
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet, Route("post")]
        public async Task<ServiceResult<PostDetailDto>> GetPostDetailAsync(string url)
        {
            return await _blogService.GetPostDetailAsync(url);
        }

        /// <summary>
        /// 通过分类名称查询文章列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("posts/category")]
        public async Task<ServiceResult<IEnumerable<QueryPostDto>>> QueryPostsByCategoryAsync([Required] string name)
        {
            return await _blogService.QueryPostsByCategoryAsync(name);
        }

        /// <summary>
        /// 通过标签名称查询文章列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("posts/tag")]
        public async Task<ServiceResult<IEnumerable<QueryPostDto>>> QueryPostsByTagAsync([Required] string name)
        {
            return await _blogService.QueryPostsByTagAsync(name);
        }

        #endregion Posts

        #region Categories

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("categories")]
        public async Task<ServiceResult<IEnumerable<QueryCategoryDto>>> QueryCategoriesAsync()
        {
            return await _blogService.QueryCategoriesAsync();
        }

        /// <summary>
        /// 获取分类名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("category")]
        public async Task<ServiceResult<string>> GetCategoryAsync([Required] string name)
        {
            return await _blogService.GetCategoryAsync(name);
        }

        #endregion Categories

        #region Tags

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("tags")]
        public async Task<ServiceResult<IEnumerable<QueryTagDto>>> QueryTagsAsync()
        {
            return await _blogService.QueryTagsAsync();
        }

        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("tag")]
        public async Task<ServiceResult<string>> GetTagAsync([Required] string name)
        {
            return await _blogService.GetTagAsync(name);
        }

        #endregion Tags

        #region FriendLinks

        /// <summary>
        /// 查询友链列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("friendLinks")]
        public async Task<ServiceResult<IEnumerable<FriendLinkDto>>> QueryFriendLinksAsync()
        {
            return await _blogService.QueryFriendLinksAsync();
        }

        #endregion FriendLinks
    }
}