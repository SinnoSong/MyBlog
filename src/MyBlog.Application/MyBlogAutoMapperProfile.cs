using AutoMapper;
using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Blog;

namespace MyBlog.Application
{
    public class MyBlogAutoMapperProfile : Profile
    {
        public MyBlogAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}