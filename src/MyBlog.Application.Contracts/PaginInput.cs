using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog
{
    /// <summary>
    /// 分页输入参数
    /// </summary>
    public class PaginInput
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(10, 30)]
        public int Limit { get; set; } = 10;
    }
}