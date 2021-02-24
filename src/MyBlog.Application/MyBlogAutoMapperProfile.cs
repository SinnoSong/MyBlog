using AutoMapper;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Application.Contracts.Blog.Params;
using MyBlog.Domain.Blog;

namespace MyBlog.Application
{
    public class MyBlogAutoMapperProfile : Profile
    {
        public MyBlogAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<EditPostInput, Post>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Post, PostForAdminDto>().ForMember(x => x.Tags, opt => opt.Ignore());
        }
    }
}