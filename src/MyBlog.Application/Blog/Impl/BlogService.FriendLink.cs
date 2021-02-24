using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Blog;
using MyBolg.ToolKits.Base;
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
        /// 查询友链列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<FriendLinkDto>>> QueryFriendLinksAsync()
        {
            return await _blogCacheService.QueryFriendLinksAsync(async () =>
            {
                var result = new ServiceResult<IEnumerable<FriendLinkDto>>();

                var entityList = await _friendLinksRepository.GetListAsync();
                var dtoList = ObjectMapper.Map<IEnumerable<FriendLink>, IEnumerable<FriendLinkDto>>(entityList);

                result.IsSuccess(dtoList);
                return result;
            });
        }
    }
}