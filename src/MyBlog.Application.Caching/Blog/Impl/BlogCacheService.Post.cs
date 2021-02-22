﻿using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System;
using System.Threading.Tasks;

namespace MyBlog.Application.Caching.Blog.Impl
{
    public partial class BlogCacheService
    {
        private const string KEY_QueryPosts = "Blog:Post:Query-{0}-{1}";
        private const string KEY_QueryPostDetail = "Blog:Post:GetPostDetail-{0}";

        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input, Func<Task<ServiceResult<PagedList<QueryPostDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_QueryPosts.FormatWith(input.Page, input.Limit), factory, CacheStrategy.ONE_DAY);
        }

        /// <summary>
        /// 根据URl获取文章详情
        /// </summary>
        /// <param name="url"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PostDetailDto>> GetPostDetailAsync(string url, Func<Task<ServiceResult<PostDetailDto>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_QueryPostDetail, factory, CacheStrategy.ONE_DAY);
        }
    }
}