﻿using MyBlog.Application.Contracts.Blog;
using MyBolg.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Application.Caching.Blog
{
    public partial interface IBlogCacheService
    {
        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QueryCategoryDto>>> QueryCategoriesAsync(Func<Task<ServiceResult<IEnumerable<QueryCategoryDto>>>> factory);

        /// <summary>
        /// 获取分类名称
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetCategoryAsync(string name, Func<Task<ServiceResult<string>>> factory);
    }
}