using MyBlog.Domain.Blog;
using Volo.Abp.Domain.Repositories;

namespace MyBlog.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post, int>
    {
    }
}