namespace MyBlog.Application.Contracts.Blog
{
    public class PostBriefDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int Year { get; set; }
        public string CreationTime { get; set; }
    }
}