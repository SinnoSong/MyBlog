using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Blog
{
    public class QueryPostDto
    {
        public int Year { get; set; }
        public IEnumerable<PostBrieDto> Posts { get; set; }
    }
}