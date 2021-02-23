using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Blog;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MyBlog.HttpApi.Controllers
{
    [ApiController, ApiExplorerSettings(GroupName = Grouping.GroupName_v1)]
    [Route("[controller]"), Authorize]
    public class BlogController : AbpController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

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
        /// 查询分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("categories")]
        public async Task<ServiceResult<IEnumerable<QueryCategoryDto>>> QueryCategoriesAsync()
        {
            return await _blogService.QueryCategoriesAsync();
        }
    }
}