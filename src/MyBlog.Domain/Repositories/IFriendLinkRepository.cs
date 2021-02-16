using MyBlog.Domain.Blog;
using Volo.Abp.Domain.Repositories;

namespace MyBlog.Domain.Repositories
{
    /// <summary>
    /// IFriendLinkRepository
    /// </summary>
    public interface IFriendLinkRepository : IRepository<FriendLink, int>
    {
    }
}