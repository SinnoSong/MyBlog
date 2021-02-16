using MyBlog.Domain.Blog;
using Volo.Abp.Domain.Repositories;

namespace MyBlog.Domain.Repositories
{
    /// <summary>
    /// ICategoryRepository
    /// </summary>
    public interface ICategoryRepository : IRepository<Category, int>
    {
    }
}