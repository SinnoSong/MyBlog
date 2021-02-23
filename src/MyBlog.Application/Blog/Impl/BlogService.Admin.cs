using MyBlog.Application.Contracts.Blog;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostForAdminDto>>> QueryPostsForAdminAsync(PaginInput input)
        {
            var result = new ServiceResult<PagedList<QueryPostForAdminDto>>();

            var count = await _postRepository.GetCountAsync();

            var list = _postRepository.OrderByDescending(x => x.CreationTime)
                .PageByIndex(input.Page, input.Limit)
                .Select(x => new PostBriefForAdminDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url,
                    Year = x.CreationTime.Year,
                    CreationTime = x.CreationTime.TryToDateTime()
                }).GroupBy(x => x.Year)
                .Select(x => new QueryPostForAdminDto
                {
                    Year = x.Key,
                    Posts = x.ToList()
                }).ToList();
            result.IsSuccess(new PagedList<QueryPostForAdminDto>(count.TryToInt(), list));
            return result;
        }
    }
}