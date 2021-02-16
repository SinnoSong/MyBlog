using MyBlog.Application.Contracts.Blog;
using MyBolg.ToolKits.Base;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog
{
    public interface IBlogService
    {
        Task<ServiceResult<string>> InsertPostAsync(PostDto dto);

        Task<ServiceResult> DeletePostAsync(int id);

        Task<ServiceResult<string>> UpdatePostAsync(int id, PostDto dto);

        Task<ServiceResult<PostDto>> GetPostAsync(int id);
    }
}