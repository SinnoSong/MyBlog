using System.Collections.Generic;

namespace MyBlog.Application.Contracts.Blog.Params
{
    public class EditPostInput : PostDto
    {
        /// <summary>
        /// 标签列表
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
    }
}