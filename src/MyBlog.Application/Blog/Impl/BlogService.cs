using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Blog;
using MyBlog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<bool> DeletePostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PostDto> GetPostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertPostAsync(PostDto dto)
        {
            var entity = new Post
            {
                Title = dto.Title,
                Author = dto.Author,
                Url = dto.Url,
                Html = dto.Html,
                Markdown = dto.Markdown,
                CategoryId = dto.CategoryId,
                CreationTime = dto.CreationTime
            };
            var post = await _postRepository.InsertAsync(entity);
            return post != null;
        }

        public Task<bool> UpdatePostAsync(int id, PostDto dto)
        {
            throw new NotImplementedException();
        }
    }
}