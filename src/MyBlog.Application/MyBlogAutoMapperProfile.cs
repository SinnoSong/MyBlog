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
            CreateMap<EditPostInput, Post>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Post, PostForAdminDto>().ForMember(x => x.Tags, opt => opt.Ignore());
            CreateMap<FriendLink, FriendLinkDto>().ReverseMap();
            CreateMap<EditCategoryInput, Category>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<EditTagInput, Tag>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}