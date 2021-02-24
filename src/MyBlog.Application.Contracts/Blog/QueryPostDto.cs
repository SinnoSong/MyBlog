using System.Collections.Generic;

namespace MyBlog.Application.Contracts.Blog
{
    public class QueryPostDto
    {
        public int Year { get; set; }
        public IEnumerable<PostBriefDto> Posts { get; set; }
    }
}