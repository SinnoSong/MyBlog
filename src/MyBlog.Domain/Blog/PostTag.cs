using Volo.Abp.Domain.Entities;

namespace MyBlog.Domain.Blog
{
    /// <summary>
    /// 文章对应标签表
    /// </summary>
    public class PostTag : Entity<int>
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public int TagId { get; set; }
    }
}