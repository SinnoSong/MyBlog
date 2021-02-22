using MyBlog.Application.Caching.Blog;
using MyBlog.Domain.Repositories;

namespace MyBlog.Application.Blog.Impl
{
    public partial class BlogService : ServiceBase, IBlogService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogCacheService _blogCacheService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagsRepository;
        private readonly IFriendLinkRepository _friendLinksRepository;

        public BlogService(IPostRepository postRepository, IBlogCacheService blogCacheService,
                            ICategoryRepository categoryRepository, ITagRepository tagRepository,
                            IPostTagRepository postTagsRepository, IFriendLinkRepository friendLinksRepository)
        {
            _postRepository = postRepository;
            _blogCacheService = blogCacheService;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _postTagsRepository = postTagsRepository;
            _friendLinksRepository = friendLinksRepository;
        }
    }
}