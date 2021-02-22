using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Blog;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
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

        [HttpGet, Route("posts")]
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync([FromQuery] PaginInput input)
        {
            return await _blogService.QueryPostAsync(input);
        }
    }
}