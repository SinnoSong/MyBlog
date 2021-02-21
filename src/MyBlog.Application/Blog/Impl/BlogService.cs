using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Blog;
using MyBlog.Domain.Repositories;
using MyBolg.ToolKits.Base;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog.Impl
{
    public class BlogService : ServiceBase, IBlogService
    {
        private readonly IPostRepository _postRepository;

        public BlogService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<ServiceResult> DeletePostAsync(int id)
        {
            var result = new ServiceResult();
            await _postRepository.DeleteAsync(id);
            return result;
        }

        public async Task<ServiceResult<PostDto>> GetPostAsync(int id)
        {
            var result = new ServiceResult<PostDto>();
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                result.IsFailed("文章不存在");
            }
            var dto = ObjectMapper.Map<Post, PostDto>(post);
            result.IsSuccess(dto);
            return result;
        }

        public async Task<ServiceResult<string>> InsertPostAsync(PostDto dto)
        {
            var result = new ServiceResult<string>();
            var entity = ObjectMapper.Map<PostDto, Post>(dto);
            var post = await _postRepository.InsertAsync(entity);
            if (post == null)
            {
                result.IsFailed("添加失败");
                return result;
            }
            result.IsSuccess("添加成功");
            return result;
        }

        public async Task<ServiceResult<string>> UpdatePostAsync(int id, PostDto dto)
        {
            var result = new ServiceResult<string>();
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                result.IsFailed("文章不存在");
                return result;
            }
            ObjectMapper.Map(dto, post);
            await _postRepository.UpdateAsync(post);
            result.IsSuccess("更新成功");
            return result;
        }
    }
}