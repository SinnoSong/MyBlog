namespace MyBlog.Application.Contracts.Blog
{
    public class QueryCategoryDto : CategoryDto
    {
        public int Count { get; set; }
    }
}