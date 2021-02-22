using MyBlog.Blog;
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
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input)
        {
            return await _blogCacheService.QueryPostAsync(input, async () =>
             {
                 var result = new ServiceResult<PagedList<QueryPostDto>>();

                 var count = await _postRepository.GetCountAsync();

                 var list = _postRepository.OrderByDescending(x => x.CreationTime)
                                           .PageByIndex(input.Page, input.Limit)
                                           .Select(x => new PostBrieDto
                                           {
                                               Title = x.Title,
                                               Url = x.Url,
                                               Year = x.CreationTime.Year,
                                               CreateTime = x.CreationTime.TryToDateTime()
                                           }).GroupBy(x => x.Year)
                                           .Select(x => new QueryPostDto
                                           {
                                               Year = x.Key,
                                               Posts = x.ToList()
                                           }).ToList();
                 result.IsSuccess(new PagedList<QueryPostDto>(count.TryToInt(), list));
                 return result;
             });
        }
    }
}